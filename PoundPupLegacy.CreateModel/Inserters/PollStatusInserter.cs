namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollStatusInserterFactory : DatabaseInserterFactory<PollStatus>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override async Task<IDatabaseInserter<PollStatus>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_status",
            new DatabaseParameter[] {
                Id,
                Name
            }
        );
        return new PollStatusInserter(command);
    }
}
internal sealed class PollStatusInserter : DatabaseInserter<PollStatus>
{
    internal const string ID = "id";
    internal const string NAME = "name";
    internal PollStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollStatus pollStatus)
    {
        if (pollStatus.Id is null)
            throw new NullReferenceException();
        Set(PollStatusInserterFactory.Id, pollStatus.Id.Value);
        Set(PollStatusInserterFactory.Name, pollStatus.Name);
        await _command.ExecuteNonQueryAsync();
    }
}
