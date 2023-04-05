namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreator : EntityCreator<SingleQuestionPoll>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Poll> _pollInserterFactory;
    private readonly IDatabaseInserterFactory<SingleQuestionPoll> _singleQuestionPollInserterFactory;
    private readonly IDatabaseInserterFactory<PollQuestion> _pollQuestionInserterFactory;
    private readonly IDatabaseInserterFactory<PollOption> _pollOptionInserterFactory;
    private readonly IDatabaseInserterFactory<PollVote> _pollVoteInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public SingleQuestionPollCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory, 
        IDatabaseInserterFactory<Searchable> searchableInserterFactory, 
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory, 
        IDatabaseInserterFactory<Poll> pollInserterFactory, 
        IDatabaseInserterFactory<SingleQuestionPoll> singleQuestionPollInserterFactory, 
        IDatabaseInserterFactory<PollQuestion> pollQuestionInserterFactory, 
        IDatabaseInserterFactory<PollOption> pollOptionInserterFactory, 
        IDatabaseInserterFactory<PollVote> pollVoteInserterFactory, 
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _pollInserterFactory = pollInserterFactory;
        _singleQuestionPollInserterFactory = singleQuestionPollInserterFactory;
        _pollQuestionInserterFactory = pollQuestionInserterFactory;
        _pollOptionInserterFactory = pollOptionInserterFactory;
        _pollVoteInserterFactory = pollVoteInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<SingleQuestionPoll> polls, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var pollWriter = await _pollInserterFactory.CreateAsync(connection);
        await using var pollQuestionWriter = await _pollQuestionInserterFactory.CreateAsync(connection);
        await using var singleQuestionPollWriter = await _singleQuestionPollInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);
        await using var pollOptionWriter = await _pollOptionInserterFactory.CreateAsync(connection);
        await using var pollVoteWriter = await _pollVoteInserterFactory.CreateAsync(connection);

        await foreach (var poll in polls) {
            await nodeWriter.InsertAsync(poll);
            await searchableWriter.InsertAsync(poll);
            await simpleTextNodeWriter.InsertAsync(poll);
            await pollWriter.InsertAsync(poll);
            await pollQuestionWriter.InsertAsync(poll);
            await singleQuestionPollWriter.InsertAsync(poll);
            foreach (var tenantNode in poll.TenantNodes) {
                tenantNode.NodeId = poll.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
            foreach (var pollOption in poll.PollOptions) {
                pollOption.PollQuestionId = poll.Id;
                await pollOptionWriter.InsertAsync(pollOption);
            }
            foreach (var pollVote in poll.PollVotes) {
                pollVote.PollId = poll.Id;
                await pollVoteWriter.InsertAsync(pollVote);
            }

        }
    }
}
