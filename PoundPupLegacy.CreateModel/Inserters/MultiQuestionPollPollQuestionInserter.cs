namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MultiQuestionPollPollQuestionInserter : DatabaseInserter<MultiQuestionPollPollQuestion>, IDatabaseInserter<MultiQuestionPollPollQuestion>
{

    private const string MULTI_QUESTION_POLL_ID = "multi_question_poll_id";
    private const string POLL_QUESTION_ID = "poll_question_id";
    private const string DELTA = "delta";
    public static async Task<DatabaseInserter<MultiQuestionPollPollQuestion>> CreateAsync(NpgsqlConnection connection)
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
        return new MultiQuestionPollPollQuestionInserter(command);

    }

    internal MultiQuestionPollPollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(MultiQuestionPollPollQuestion question)
    {
        WriteValue(question.MultiQuestionPollId, MULTI_QUESTION_POLL_ID);
        WriteNullableValue(question.PollQuestionId, POLL_QUESTION_ID);
        WriteNullableValue(question.Delta, DELTA);
        await _command.ExecuteNonQueryAsync();
    }
}
