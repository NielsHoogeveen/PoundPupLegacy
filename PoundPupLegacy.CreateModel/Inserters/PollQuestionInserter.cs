namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollQuestionInserterFactory : DatabaseInserterFactory<PollQuestion>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Question = new() { Name = "question" };

    public override async Task<IDatabaseInserter<PollQuestion>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_question",
            new DatabaseParameter[] {
                Id,
                Question
            }
        );
        return new PollQuestionInserter(command);
    }
}
internal sealed class PollQuestionInserter : DatabaseInserter<PollQuestion>
{
    internal PollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollQuestion pollQuestion)
    {
        if (pollQuestion.Id is null)
            throw new NullReferenceException();
        Set(PollQuestionInserterFactory.Id, pollQuestion.Id.Value);
        Set(PollQuestionInserterFactory.Question, pollQuestion.Question);
        await _command.ExecuteNonQueryAsync();
    }
}
