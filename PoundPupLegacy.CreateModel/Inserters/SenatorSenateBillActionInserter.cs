using System.Collections.Immutable;
namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SenatorSenateBillActionInserterFactory : DatabaseInserterFactory<SenatorSenateBillAction>
{
    public override async Task<IDatabaseInserter<SenatorSenateBillAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var columnDefinitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = SenatorSenateBillActionInserter.SENATOR_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SenatorSenateBillActionInserter.SENATE_BILL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SenatorSenateBillActionInserter.BILL_ACTION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SenatorSenateBillActionInserter.DATE,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
            };

        var genarateIdCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "senator_senate_bill_action",
            columnDefinitions
        );

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "senator_senate_bill_action",
            columnDefinitions.ToImmutableList().Add(new ColumnDefinition {
                Name = SenatorSenateBillActionInserter.ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        return new SenatorSenateBillActionInserter(command, genarateIdCommand);
    }
}
internal sealed class SenatorSenateBillActionInserter : DatabaseInserter<SenatorSenateBillAction>
{

    internal const string ID = "id";
    internal const string SENATOR_ID = "senator_id";
    internal const string SENATE_BILL_ID = "senate_bill_id";
    internal const string DATE = "date";
    internal const string BILL_ACTION_TYPE_ID = "bill_action_type_id";

    private NpgsqlCommand _generateIdCommand;

    internal SenatorSenateBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command)
    {
        _generateIdCommand = generateIdCommand;
    }

    private void DoWrites(SenatorSenateBillAction senatorSenateBillAction, NpgsqlCommand command)
    {
        WriteValue(senatorSenateBillAction.SenatorId, SENATOR_ID, command);
        WriteValue(senatorSenateBillAction.SenateBillId, SENATE_BILL_ID, command);
        WriteValue(senatorSenateBillAction.BillActionTypeId, BILL_ACTION_TYPE_ID, command);
        WriteValue(senatorSenateBillAction.Date, DATE, command);

    }

    public override async Task InsertAsync(SenatorSenateBillAction senatorSenateBillAction)
    {
        if (senatorSenateBillAction.Id is null) {
            DoWrites(senatorSenateBillAction, _generateIdCommand);
            senatorSenateBillAction.Id = await _command.ExecuteScalarAsync() switch {
                long i => (int)i,
                _ => throw new Exception("Insert of senator senate bill action does not return an id.")
            };
        }
        else {
            WriteValue(senatorSenateBillAction.Id, ID);
            DoWrites(senatorSenateBillAction, _command);
            await _command.ExecuteNonQueryAsync();
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await _generateIdCommand.DisposeAsync();
        await base.DisposeAsync();
    }
}
