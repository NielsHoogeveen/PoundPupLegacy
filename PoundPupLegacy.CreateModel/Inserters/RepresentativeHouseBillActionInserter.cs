using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class RepresentativeHouseBillActionInserter : DatabaseInserter<RepresentativeHouseBillAction>, IDatabaseInserter<RepresentativeHouseBillAction>
{

    private const string ID = "id";
    private const string REPRESENTATIVE_ID = "representative_id";
    private const string HOUSE_BILL_ID = "house_bill_id";
    private const string DATE = "date";
    private const string BILL_ACTION_TYPE_ID = "bill_action_type_id";
    public static async Task<DatabaseInserter<RepresentativeHouseBillAction>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = REPRESENTATIVE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HOUSE_BILL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BILL_ACTION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
            };

        var genarateIdCommand = await CreateIdentityInsertStatementAsync(
            connection,
            "representative_house_bill_action",
            columnDefinitions
        );

        var command = await CreateInsertStatementAsync(
            connection,
            "representative_house_bill_action",
            columnDefinitions.ToImmutableList().Add(new ColumnDefinition {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        return new RepresentativeHouseBillActionInserter(command, genarateIdCommand);

    }

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
