namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MultiQuestionPollInserter : IDatabaseInserter<MultiQuestionPoll>
{
    public static async Task<DatabaseInserter<MultiQuestionPoll>> CreateAsync(IDbConnection connection)
    {
        return await SingleIdInserter.CreateSingleIdWriterAsync<MultiQuestionPoll>("multi_question_poll", connection);
    }
}
