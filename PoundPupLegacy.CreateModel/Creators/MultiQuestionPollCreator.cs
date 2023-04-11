namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class MultiQuestionPollCreator : EntityCreator<MultiQuestionPoll>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<Poll> _pollInserterFactory;
    private readonly IDatabaseInserterFactory<MultiQuestionPoll> _multiQuestionPollInserterFactory;
    private readonly IDatabaseInserterFactory<PollQuestion> _pollQuestionInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IDatabaseInserterFactory<PollOption> _pollOptionInserterFactory;
    private readonly IDatabaseInserterFactory<PollVote> _pollVoteInserterFactory;
    private readonly IDatabaseInserterFactory<MultiQuestionPollPollQuestion> _multiQuestionPollPollQuestionInserterFactory;
    public MultiQuestionPollCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
        IDatabaseInserterFactory<Poll> pollInserterFactory,
        IDatabaseInserterFactory<MultiQuestionPoll> multiQuestionPollInserterFactory,
        IDatabaseInserterFactory<PollQuestion> pollQuestionInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IDatabaseInserterFactory<PollOption> pollOptionInserterFactory,
        IDatabaseInserterFactory<PollVote> pollVoteInserterFactory,
        IDatabaseInserterFactory<MultiQuestionPollPollQuestion> multiQuestionPollPollQuestionInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _pollInserterFactory = pollInserterFactory;
        _multiQuestionPollInserterFactory = multiQuestionPollInserterFactory;
        _pollQuestionInserterFactory = pollQuestionInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _pollOptionInserterFactory = pollOptionInserterFactory;
        _pollVoteInserterFactory = pollVoteInserterFactory;
        _multiQuestionPollPollQuestionInserterFactory = multiQuestionPollPollQuestionInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<MultiQuestionPoll> polls, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var pollWriter = await _pollInserterFactory.CreateAsync(connection);
        await using var multiQuesionPollWriter = await _multiQuestionPollInserterFactory.CreateAsync(connection);
        await using var pollQuestionWriter = await _pollQuestionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);
        await using var pollOptionWriter = await _pollOptionInserterFactory.CreateAsync(connection);
        await using var pollVoteWriter = await _pollVoteInserterFactory.CreateAsync(connection);
        await using var multiQuestionPollPollQuestionWriter = await _multiQuestionPollPollQuestionInserterFactory.CreateAsync(connection);

        await foreach (var poll in polls) {
            await nodeWriter.InsertAsync(poll);
            await searchableWriter.InsertAsync(poll);
            await simpleTextNodeWriter.InsertAsync(poll);
            await pollWriter.InsertAsync(poll);
            await multiQuesionPollWriter.InsertAsync(poll);
            foreach (var (question, index) in poll.PollQuestions.Select((q, i) => (q, i))) {
                await nodeWriter.InsertAsync(question);
                await searchableWriter.InsertAsync(question);
                await simpleTextNodeWriter.InsertAsync(question);
                await pollQuestionWriter.InsertAsync(question);

                var pollQuestions = new MultiQuestionPollPollQuestion {
                    MultiQuestionPollId = poll.Id!.Value,
                    PollQuestionId = question.Id!.Value,
                    Delta = index
                };
                await multiQuestionPollPollQuestionWriter.InsertAsync(pollQuestions);
                foreach (var pollOption in question.PollOptions) {
                    pollOption.PollQuestionId = question.Id;
                    await pollOptionWriter.InsertAsync(pollOption);
                }
                foreach (var pollVote in question.PollVotes) {
                    pollVote.PollId = question.Id;
                    await pollVoteWriter.InsertAsync(pollVote);
                }

                foreach (var tenantNode in question.TenantNodes) {
                    tenantNode.NodeId = question.Id;
                    await tenantNodeWriter.InsertAsync(tenantNode);
                }

            }

            foreach (var tenantNode in poll.TenantNodes) {
                tenantNode.NodeId = poll.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
