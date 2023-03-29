namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollQuestionInserter : DatabaseInserter<PollQuestion>, IDatabaseInserter<PollQuestion>
{
    private const string ID = "id";
    private const string QUESTION = "question";

    public static async Task<DatabaseInserter<PollQuestion>> CreateAsync(NpgsqlConnection connection)
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
        return new PollQuestionInserter(command);

    }

    internal PollQuestionInserter(NpgsqlCommand command) : base(command)
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
