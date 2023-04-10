namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollInserterFactory : DatabaseInserterFactory<Poll>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableDateTimeDatabaseParameter DateTimeClosure = new() { Name = "date_time_closure" };
    internal static NonNullableIntegerDatabaseParameter PollStatusId = new() { Name = "poll_status_id" };

    public override async Task<IDatabaseInserter<Poll>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll",
            new DatabaseParameter[] {
                Id,
                DateTimeClosure,
                PollStatusId
            }
        );
        return new PollInserter(command);
    }
}
internal sealed class PollInserter : DatabaseInserter<Poll>
{
    internal PollInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Poll poll)
    {
        if (poll.Id is null)
            throw new NullReferenceException();
        Set(PollInserterFactory.Id, poll.Id.Value);
        Set(PollInserterFactory.DateTimeClosure, poll.DateTimeClosure);
        Set(PollInserterFactory.PollStatusId, poll.PollStatusId);
        await _command.ExecuteNonQueryAsync();
    }
}
