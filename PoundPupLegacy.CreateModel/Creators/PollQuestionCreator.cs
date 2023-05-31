namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PollQuestionCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNodeToCreate> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<PollQuestionToCreate> pollQuestionInserterFactory,
    IDatabaseInserterFactory<PollOption> pollOptionInserterFactory,
    IDatabaseInserterFactory<PollVote> pollVoteInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PollQuestionToCreate>
{
    public async Task<IEntityCreator<PollQuestionToCreate>> CreateAsync(IDbConnection connection) =>
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
    List<IDatabaseInserter<PollQuestionToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    IDatabaseInserter<PollOption> pollOptionInserter,
    IDatabaseInserter<PollVote> pollVoteInserter
) : NodeCreator<PollQuestionToCreate>(
        inserters,
        nodeDetailsCreator
)
{
    public override async Task ProcessAsync(PollQuestionToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        foreach (var pollOption in element.PollQuestionDetails.PollOptions) {
            pollOption.PollQuestionId = id;
            await pollOptionInserter.InsertAsync(pollOption);
        }
        foreach (var pollVote in element.PollQuestionDetails.PollVotes) {
            pollVote.PollId = id;
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