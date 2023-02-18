namespace PoundPupLegacy.Db.Writers;

internal sealed class PollQuestionWriter : DatabaseWriter<PollQuestion>, IDatabaseWriter<PollQuestion>
{
    private const string ID = "id";
    private const string QUESTION = "question";
    
    public static async Task<DatabaseWriter<PollQuestion>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "poll_question",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = QUESTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new PollQuestionWriter(command);

    }

    internal PollQuestionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(PollQuestion pollQuestion)
    {
        if (pollQuestion.Id is null)
            throw new NullReferenceException();
        WriteValue(pollQuestion.Id, ID);
        WriteValue(pollQuestion.Question, QUESTION);
        await _command.ExecuteNonQueryAsync();
    }
}
