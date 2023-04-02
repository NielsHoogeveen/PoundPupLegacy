namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollOptionInserter : DatabaseInserter<PollOption>, IDatabaseInserter<PollOption>
{
    private const string POLL_QUESTION_ID = "poll_question_id";
    private const string DELTA = "delta";
    private const string TEXT = "text";
    private const string NUMBER_OF_VOTES = "number_of_votes";
    public static async Task<DatabaseInserter<PollOption>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_option",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = POLL_QUESTION_ID,
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
        return new PollOptionInserter(command);

    }

    internal PollOptionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollOption pollOption)
    {
        if (pollOption.PollQuestionId is null)
            throw new NullReferenceException();
        WriteValue(pollOption.PollQuestionId, POLL_QUESTION_ID);
        WriteValue(pollOption.Delta, DELTA);
        WriteNullableValue(pollOption.Text, TEXT);
        WriteNullableValue(pollOption.NumberOfVotes, NUMBER_OF_VOTES);
        await _command.ExecuteNonQueryAsync();
    }
}
