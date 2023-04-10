namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicActionInserterFactory : DatabaseInserterFactory<BasicAction>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };


    public override async Task<IDatabaseInserter<BasicAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "basic_action",
            new DatabaseParameter[] {
                Id,
                Path,
                Description
            }
        );
        return new BasicActionInserter(command);

    }

}
internal sealed class BasicActionInserter : DatabaseInserter<BasicAction>
{


    internal BasicActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BasicAction actionAccessPrivilege)
    {
        if (!actionAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        Set(BasicActionInserterFactory.Id, actionAccessPrivilege.Id.Value);
        Set(BasicActionInserterFactory.Path, actionAccessPrivilege.Path);
        Set(BasicActionInserterFactory.Description, actionAccessPrivilege.Description);
        await _command.ExecuteNonQueryAsync();
    }
}
