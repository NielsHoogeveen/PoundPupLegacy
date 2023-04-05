namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class MultiQuestionPollPollQuestionInserterFactory : DatabaseInserterFactory<MultiQuestionPollPollQuestion>
{
    public override async Task<IDatabaseInserter<MultiQuestionPollPollQuestion>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "multi_question_poll_poll_question",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = MultiQuestionPollPollQuestionInserter.MULTI_QUESTION_POLL_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = MultiQuestionPollPollQuestionInserter.POLL_QUESTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = MultiQuestionPollPollQuestionInserter.DELTA,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new MultiQuestionPollPollQuestionInserter(command);
    }
}
internal sealed class MultiQuestionPollPollQuestionInserter : DatabaseInserter<MultiQuestionPollPollQuestion>
{

    internal const string MULTI_QUESTION_POLL_ID = "multi_question_poll_id";
    internal const string POLL_QUESTION_ID = "poll_question_id";
    internal const string DELTA = "delta";

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
