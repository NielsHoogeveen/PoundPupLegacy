namespace PoundPupLegacy.Db.Writers;

internal sealed class PollOptionWriter : DatabaseWriter<PollOption>, IDatabaseWriter<PollOption>
{
    private const string POLL_ID = "poll_id";
    private const string DELTA = "delta";
    private const string TEXT = "text";
    private const string NUMBER_OF_VOTES = "number_of_votes";
    public static async Task<DatabaseWriter<PollOption>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "poll_option",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = POLL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DELTA,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = NUMBER_OF_VOTES,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PollOptionWriter(command);

    }

    internal PollOptionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(PollOption pollOption)
    {
        if (pollOption.PollId is null)
            throw new NullReferenceException();
        WriteValue(pollOption.PollId, POLL_ID);
        WriteValue(pollOption.Delta, DELTA);
        WriteNullableValue(pollOption.Text, TEXT);
        WriteNullableValue(pollOption.NumberOfVotes, NUMBER_OF_VOTES);
        await _command.ExecuteNonQueryAsync();
    }
}
