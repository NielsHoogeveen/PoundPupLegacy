namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CollectiveUserInserterFactory : DatabaseInserterFactory<CollectiveUser>
{
    internal static NonNullableIntegerDatabaseParameter CollectiveId = new() { Name = "collective_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    public override async Task<IDatabaseInserter<CollectiveUser>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "collective_user",
            new DatabaseParameter[] {
                CollectiveId,
                UserId
            }
        );
        return new CollectiveUserInserter(command);

    }

}
internal sealed class CollectiveUserInserter : DatabaseInserter<CollectiveUser>
{
    internal CollectiveUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CollectiveUser collectiveUser)
    {
        if (collectiveUser.CollectiveId is null || collectiveUser.UserId is null)
            throw new NullReferenceException();
        Set(CollectiveUserInserterFactory.CollectiveId, collectiveUser.CollectiveId.Value);
        Set(CollectiveUserInserterFactory.UserId, collectiveUser.UserId.Value);
        await _command.ExecuteNonQueryAsync();
    }
}
