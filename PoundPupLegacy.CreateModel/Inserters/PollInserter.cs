namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollInserterFactory : DatabaseInserterFactory<Poll>
{
    public override async Task<IDatabaseInserter<Poll>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PollInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollInserter.DATE_TIME_CLOSURE,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = PollInserter.POLL_STATUS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PollInserter(command);
    }
}
internal sealed class PollInserter : DatabaseInserter<Poll>
{
    internal const string ID = "id";
    internal const string DATE_TIME_CLOSURE = "date_time_closure";
    internal const string POLL_STATUS_ID = "poll_status_id";

    internal PollInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Poll poll)
    {
        if (poll.Id is null)
            throw new NullReferenceException();
        WriteValue(poll.Id, ID);
        WriteValue(poll.DateTimeClosure, DATE_TIME_CLOSURE);
        WriteValue(poll.PollStatusId, POLL_STATUS_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
