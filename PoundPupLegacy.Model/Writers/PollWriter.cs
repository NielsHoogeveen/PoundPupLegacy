namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class PollWriter : DatabaseWriter<Poll>, IDatabaseWriter<Poll>
{
    private const string ID = "id";
    private const string DATE_TIME_CLOSURE = "date_time_closure";
    private const string POLL_STATUS_ID = "poll_status_id";
    public static async Task<DatabaseWriter<Poll>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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
        return new PollWriter(command);

    }

    internal PollWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Poll poll)
    {
        if (poll.Id is null)
            throw new NullReferenceException();
        WriteValue(poll.Id, ID);
        WriteValue(poll.DateTimeClosure, DATE_TIME_CLOSURE);
        WriteValue(poll.PollStatusId, POLL_STATUS_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
