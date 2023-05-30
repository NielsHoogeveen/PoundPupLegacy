namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class MultiQuestionPollCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<PollToCreate> pollInserterFactory,
    IDatabaseInserterFactory<MultiQuestionPoll.ToCreate> multiQuestionPollInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<PollQuestionToCreate> pollQuestionCreatorFactory,
    IDatabaseInserterFactory<MultiQuestionPollPollQuestion> multiQuestionPollPollQuestionInserterFactory
) : IEntityCreatorFactory<MultiQuestionPoll.ToCreate>
{
    public async Task<IEntityCreator<MultiQuestionPoll.ToCreate>> CreateAsync(IDbConnection connection) =>
        new MultiQuestionPollCreator(
            new (){
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await pollInserterFactory.CreateAsync(connection),
                await multiQuestionPollInserterFactory.CreateAsync(connection)

            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await pollQuestionCreatorFactory.CreateAsync(connection),
            await multiQuestionPollPollQuestionInserterFactory.CreateAsync(connection)
        );
}

public class MultiQuestionPollCreator(
    List<IDatabaseInserter<MultiQuestionPoll.ToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IEntityCreator<PollQuestionToCreate> pollQuestionCreatorFactory,
    IDatabaseInserter<MultiQuestionPollPollQuestion> multiQuestionPollPollQuestionInserter
    ) : NodeCreator<MultiQuestionPoll.ToCreate>(inserters, nodeDetailsCreator)
{

    public override async Task ProcessAsync(MultiQuestionPoll.ToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        foreach (var (question, index) in element.MultiQuestionPollDetails.PollQuestions.Select((q, i) => (q, i))) {
            await pollQuestionCreatorFactory.CreateAsync(question);
            var pollQuestions = new MultiQuestionPollPollQuestion {
                MultiQuestionPollId = id,
                PollQuestionId = question.IdentificationForCreate.Id!.Value,
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