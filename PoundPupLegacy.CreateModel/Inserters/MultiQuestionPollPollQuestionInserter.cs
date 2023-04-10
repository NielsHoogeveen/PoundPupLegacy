namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class MultiQuestionPollPollQuestionInserterFactory : DatabaseInserterFactory<MultiQuestionPollPollQuestion>
{
    internal static NonNullableIntegerDatabaseParameter MultiQuestionPollId = new() { Name = "multi_question_poll_id" };
    internal static NonNullableIntegerDatabaseParameter PollQuesionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };

    public override async Task<IDatabaseInserter<MultiQuestionPollPollQuestion>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "multi_question_poll_poll_question",
            new DatabaseParameter[] {
                MultiQuestionPollId,
                PollQuesionId,
                Delta
            }
        );
        return new MultiQuestionPollPollQuestionInserter(command);
    }
}
internal sealed class MultiQuestionPollPollQuestionInserter : DatabaseInserter<MultiQuestionPollPollQuestion>
{
    internal MultiQuestionPollPollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(MultiQuestionPollPollQuestion question)
    {
        Set(MultiQuestionPollPollQuestionInserterFactory.MultiQuestionPollId, question.MultiQuestionPollId);
        Set(MultiQuestionPollPollQuestionInserterFactory.PollQuesionId, question.PollQuestionId);
        Set(MultiQuestionPollPollQuestionInserterFactory.Delta, question.Delta);
        await _command.ExecuteNonQueryAsync();
    }
}
