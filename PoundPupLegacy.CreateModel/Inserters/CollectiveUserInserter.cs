namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CollectiveUserInserterFactory : DatabaseInserterFactory<CollectiveUser>
{
    public override async Task<IDatabaseInserter<CollectiveUser>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "collective_user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CollectiveUserInserter.COLLECTIVE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CollectiveUserInserter.USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CollectiveUserInserter(command);

    }

}
internal sealed class CollectiveUserInserter : DatabaseInserter<CollectiveUser>
{
    internal const string COLLECTIVE_ID = "collective_id";
    internal const string USER_ID = "user_id";

    internal CollectiveUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CollectiveUser collectiveUser)
    {
        if (collectiveUser.CollectiveId is null || collectiveUser.UserId is null)
            throw new NullReferenceException();
        SetParameter(collectiveUser.CollectiveId, COLLECTIVE_ID);
        SetParameter(collectiveUser.UserId, USER_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
