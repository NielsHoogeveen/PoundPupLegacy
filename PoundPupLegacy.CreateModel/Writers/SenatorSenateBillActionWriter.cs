using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class SenatorSenateBillActionWriter : DatabaseWriter<SenatorSenateBillAction>, IDatabaseWriter<SenatorSenateBillAction>
{

    private const string ID = "id";
    private const string SENATOR_ID = "senator_id";
    private const string SENATE_BILL_ID = "senate_bill_id";
    private const string DATE = "date";
    private const string BILL_ACTION_TYPE_ID = "bill_action_type_id";
    public static async Task<DatabaseWriter<SenatorSenateBillAction>> CreateAsync(NpgsqlConnection connection)
    {
        var columnDefinitions = new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = SENATOR_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SENATE_BILL_ID,
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
            "senator_senate_bill_action",
            columnDefinitions
        );

        var command = await CreateInsertStatementAsync(
            connection,
            "senator_senate_bill_action",
            columnDefinitions.ToImmutableList().Add(new ColumnDefinition {
                Name = ID,
                NpgsqlDbType = NpgsqlDbType.Integer
            })
        );
        return new SenatorSenateBillActionWriter(command, genarateIdCommand);

    }

    private NpgsqlCommand _generateIdCommand;

    internal SenatorSenateBillActionWriter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command)
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

    internal override async Task WriteAsync(SenatorSenateBillAction senatorSenateBillAction)
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
