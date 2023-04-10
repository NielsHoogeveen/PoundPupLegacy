using System.Collections.Immutable;
namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SenatorSenateBillActionInserterFactory : DatabaseInserterFactory<SenatorSenateBillAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter SenatorId = new() { Name = "senator_id" };
    internal static NonNullableIntegerDatabaseParameter SenateBillId = new() { Name = "senate_bill_id" };
    internal static NonNullableDateTimeDatabaseParameter Date = new() { Name = "date" };
    internal static NonNullableIntegerDatabaseParameter BillActionTypeId = new() { Name = "bill_action_type_id" };

    public override async Task<IDatabaseInserter<SenatorSenateBillAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var databaseParameters = new DatabaseParameter[] {
            SenatorId,
            SenateBillId,
            Date,
            BillActionTypeId
        };

        var genarateIdCommand = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "senator_senate_bill_action",
            databaseParameters
        );

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "senator_senate_bill_action",
            databaseParameters.ToImmutableList().Add(Id)
        );
        return new SenatorSenateBillActionInserter(command, genarateIdCommand);
    }
}
internal sealed class SenatorSenateBillActionInserter : DatabaseInserter<SenatorSenateBillAction>
{
    private NpgsqlCommand _generateIdCommand;

    internal SenatorSenateBillActionInserter(NpgsqlCommand command, NpgsqlCommand generateIdCommand) : base(command)
    {
        _generateIdCommand = generateIdCommand;
    }

    private void DoWrites(SenatorSenateBillAction senatorSenateBillAction, NpgsqlCommand command)
    {
        Set(SenatorSenateBillActionInserterFactory.SenatorId, senatorSenateBillAction.SenatorId, command);
        Set(SenatorSenateBillActionInserterFactory.SenateBillId, senatorSenateBillAction.SenateBillId, command);
        Set(SenatorSenateBillActionInserterFactory.BillActionTypeId, senatorSenateBillAction.BillActionTypeId, command);
        Set(SenatorSenateBillActionInserterFactory.Date, senatorSenateBillAction.Date, command);
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
            Set(SenatorSenateBillActionInserterFactory.Id, senatorSenateBillAction.Id.Value);
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
