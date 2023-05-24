namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PollQuestionCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePollQuestion> pollQuestionInserterFactory,
    IDatabaseInserterFactory<PollOption> pollOptionInserterFactory,
    IDatabaseInserterFactory<PollVote> pollVoteInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiablePollQuestion>
{
    public async Task<NodeCreator<EventuallyIdentifiablePollQuestion>> CreateAsync(IDbConnection connection) =>
        new PollQuestionCreator(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await simpleTextNodeInserterFactory.CreateAsync(connection),
                await pollQuestionInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await pollOptionInserterFactory.CreateAsync(connection),
            await pollVoteInserterFactory.CreateAsync(connection)
        );
}
internal sealed class PollQuestionCreator(
    List<IDatabaseInserter<EventuallyIdentifiablePollQuestion>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IDatabaseInserter<PollOption> pollOptionInserter,
    IDatabaseInserter<PollVote> pollVoteInserter
) : NodeCreator<EventuallyIdentifiablePollQuestion>(
        inserters,
        nodeDetailsCreator
)
{
    public override async Task ProcessAsync(EventuallyIdentifiablePollQuestion element)
    {
        await base.ProcessAsync(element);
        foreach (var pollOption in element.PollOptions) {
            pollOption.PollQuestionId = element.Id;
            await pollOptionInserter.InsertAsync(pollOption);
        }
        foreach (var pollVote in element.PollVotes) {
            pollVote.PollId = element.Id;
            await pollVoteInserter.InsertAsync(pollVote);
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await pollOptionInserter.DisposeAsync();
        await pollVoteInserter.DisposeAsync();
    }
}