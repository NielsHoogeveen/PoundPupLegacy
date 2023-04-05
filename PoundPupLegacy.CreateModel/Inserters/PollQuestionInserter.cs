namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollQuestionInserterFactory : DatabaseInserterFactory<PollQuestion>
{
    public override async Task<IDatabaseInserter<PollQuestion>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_question",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PollQuestionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollQuestionInserter.QUESTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PollQuestionInserter(command);
    }
}
internal sealed class PollQuestionInserter : DatabaseInserter<PollQuestion>
{
    internal const string ID = "id";
    internal const string QUESTION = "question";

    
    internal PollQuestionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollQuestion pollQuestion)
    {
        if (pollQuestion.Id is null)
            throw new NullReferenceException();
        WriteValue(pollQuestion.Id, ID);
        WriteValue(pollQuestion.Question, QUESTION);
        await _command.ExecuteNonQueryAsync();
    }
}
