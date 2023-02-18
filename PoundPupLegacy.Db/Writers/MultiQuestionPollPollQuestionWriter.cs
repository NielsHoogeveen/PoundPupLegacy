namespace PoundPupLegacy.Db.Writers;

internal sealed class MultiQuestionPollPollQuestionWriter : DatabaseWriter<MultiQuestionPollPollQuestion>, IDatabaseWriter<MultiQuestionPollPollQuestion>
{

    private const string MULTI_QUESTION_POLL_ID = "multi_question_poll_id";
    private const string POLL_QUESTION_ID = "poll_question_id";
    private const string DELTA = "delta";
    public static async Task<DatabaseWriter<MultiQuestionPollPollQuestion>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "multi_question_poll_poll_question",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = MULTI_QUESTION_POLL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = POLL_QUESTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DELTA,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new MultiQuestionPollPollQuestionWriter(command);

    }

    internal MultiQuestionPollPollQuestionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(MultiQuestionPollPollQuestion question)
    {
        WriteValue(question.MultiQuestionPollId, MULTI_QUESTION_POLL_ID);
        WriteNullableValue(question.PollQuestionId, POLL_QUESTION_ID);
        WriteNullableValue(question.Delta, DELTA);
        await _command.ExecuteNonQueryAsync();
    }
}
