namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BillActionTypeInserterFactory : DatabaseInserterFactory<BillActionType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    public override async Task<IDatabaseInserter<BillActionType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bill_action_type",
            new DatabaseParameter[] {
                Id
            }
        );
        return new BillActionTypeInserter(command);
    }
}
internal sealed class BillActionTypeInserter : DatabaseInserter<BillActionType>
{
    internal BillActionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BillActionType billActionType)
    {
        if (billActionType.Id is null)
            throw new NullReferenceException();

        Set(BillActionTypeInserterFactory.Id, billActionType.Id.Value);
        await _command.ExecuteNonQueryAsync();
    }
}
