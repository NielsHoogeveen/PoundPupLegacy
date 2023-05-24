namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class MultiQuestionPollCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoll> pollInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableMultiQuestionPoll> multiQuestionPollInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<PollQuestion> pollQuestionCreatorFactory,
    IDatabaseInserterFactory<MultiQuestionPollPollQuestion> multiQuestionPollPollQuestionInserterFactory
) : INodeCreatorFactory<EventuallyIdentifiableMultiQuestionPoll>
{
    public async Task<NodeCreator<EventuallyIdentifiableMultiQuestionPoll>> CreateAsync(IDbConnection connection) =>
        new MultiQuestionPollCreator(
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await pollInserterFactory.CreateAsync(connection),
                await multiQuestionPollInserterFactory.CreateAsync(connection)

            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await pollQuestionCreatorFactory.CreateAsync(connection),
            await multiQuestionPollPollQuestionInserterFactory.CreateAsync(connection)
        );
}

public class MultiQuestionPollCreator(
    List<IDatabaseInserter<EventuallyIdentifiableMultiQuestionPoll>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<PollQuestion> pollQuestionCreatorFactory,
    IDatabaseInserter<MultiQuestionPollPollQuestion> multiQuestionPollPollQuestionInserter

    ) : NodeCreator<EventuallyIdentifiableMultiQuestionPoll>(inserters, nodeDetailsCreator)
{

    public override async Task ProcessAsync(EventuallyIdentifiableMultiQuestionPoll element)
    {
        await base.ProcessAsync(element);
        foreach (var (question, index) in element.PollQuestions.Select((q, i) => (q, i))) {
            await pollQuestionCreatorFactory.CreateAsync(question);
            var pollQuestions = new MultiQuestionPollPollQuestion {
                MultiQuestionPollId = element.Id!.Value,
                PollQuestionId = question.Id!.Value,
                Delta = index
            };
            await multiQuestionPollPollQuestionInserter.InsertAsync(pollQuestions);

        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await pollQuestionCreatorFactory.DisposeAsync();
        await multiQuestionPollPollQuestionInserter.DisposeAsync();
    }
}