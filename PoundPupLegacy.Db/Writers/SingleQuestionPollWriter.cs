namespace PoundPupLegacy.Db.Writers;

internal sealed class SingleQuestionPollWriter : IDatabaseWriter<SingleQuestionPoll>
{
    public static async Task<DatabaseWriter<SingleQuestionPoll>> CreateAsync(NpgsqlConnection connection)
    {
        return await SingleIdWriter.CreateSingleIdWriterAsync<SingleQuestionPoll>("single_question_poll", connection);
    }
}
