using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class RepresentativeHouseBillActionInserterFactory : DatabaseInserterFactory<RepresentativeHouseBillAction>
{
    public override async Task<IDatabaseInserter<RepresentativeHouseBillAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = RepresentativeHouseBillActionInserter.REPRESENTATIVE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = RepresentativeHouseBillActionInserter.HOUSE_BILL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = RepresentativeHouseBillActionInserter.BILL_ACTION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = RepresentativeHouseBillActionInserter.DATE,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
            };

        var genarateIdCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "representative_house_bill_action",
            columnDefinitions
        );

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "representative_house_bill_action",
            columnDefinitions.ToImmutableList().Add(new ColumnDefinition {
                Name = RepresentativeHouseBillActionInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        return new RepresentativeHouseBillActionInserter(command, genarateIdCommand);
    }
}
internal sealed class RepresentativeHouseBillActionInserter : DatabaseInserter<RepresentativeHouseBillAction>
{

    internal const string ID = "id";
    internal const string REPRESENTATIVE_ID = "representative_id";
    internal const string HOUSE_BILL_ID = "house_bill_id";
    internal const string DATE = "date";
    internal const string BILL_ACTION_TYPE_ID = "bill_action_type_id";

    private NpgsqlCommand _generateIdCommand;

    internal RepresentativeHouseBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command)
    {
        _generateIdCommand = generateIdCommand;
    }

    private void DoWrites(RepresentativeHouseBillAction representativeHouseBillAction, NpgsqlCommand command)
    {
        WriteValue(representativeHouseBillAction.RepresentativeId, REPRESENTATIVE_ID, command);
        WriteValue(representativeHouseBillAction.HouseBillId, HOUSE_BILL_ID, command);
        WriteValue(representativeHouseBillAction.BillActionTypeId, BILL_ACTION_TYPE_ID, command);
        WriteValue(representativeHouseBillAction.Date, DATE, command);

    }

    public override async Task InsertAsync(RepresentativeHouseBillAction representativeHouseBillAction)
    {
        if (representativeHouseBillAction.Id is null) {
            DoWrites(representativeHouseBillAction, _generateIdCommand);
            representativeHouseBillAction.Id = await _command.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception("Insert of representative house bill action does not return an id.")
            };
        }
        else {
            WriteValue(representativeHouseBillAction.Id, ID);
            DoWrites(representativeHouseBillAction, _command);
            await _command.ExecuteNonQueryAsync();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await _generateIdCommand.DisposeAsync();
        await base.DisposeAsync();
    }

}
