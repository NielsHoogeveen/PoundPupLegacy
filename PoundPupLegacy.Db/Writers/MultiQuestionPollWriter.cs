namespace PoundPupLegacy.Db.Writers;

internal sealed class MultiQuestionPollWriter : IDatabaseWriter<MultiQuestionPoll>
{
    public static async Task<DatabaseWriter<MultiQuestionPoll>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<MultiQuestionPoll>("multi_question_poll", connection);
    }
}
