namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SingleQuestionPollInserter : IDatabaseInserter<SingleQuestionPoll>
{
    public static async Task<DatabaseInserter<SingleQuestionPoll>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<SingleQuestionPoll>("single_question_poll", connection);
    }
}
