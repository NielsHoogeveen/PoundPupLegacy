namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollInserter : DatabaseInserter<Poll>, IDatabaseInserter<Poll>
{
    private const string ID = "id";
    private const string DATE_TIME_CLOSURE = "date_time_closure";
    private const string POLL_STATUS_ID = "poll_status_id";
    public static async Task<DatabaseInserter<Poll>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_TIME_CLOSURE,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = POLL_STATUS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PollInserter(command);

    }

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
