namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory,
    IDatabaseInserterFactory<Poll> pollInserterFactory,
    IDatabaseInserterFactory<NewSingleQuestionPoll> singleQuestionPollInserterFactory,
    IDatabaseInserterFactory<PollQuestion> pollQuestionInserterFactory,
    IDatabaseInserterFactory<PollOption> pollOptionInserterFactory,
    IDatabaseInserterFactory<PollVote> pollVoteInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewSingleQuestionPoll>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewSingleQuestionPoll> polls, IDbConnection connection)
    {

        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var pollWriter = await pollInserterFactory.CreateAsync(connection);
        await using var pollQuestionWriter = await pollQuestionInserterFactory.CreateAsync(connection);
        await using var singleQuestionPollWriter = await singleQuestionPollInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var pollOptionWriter = await pollOptionInserterFactory.CreateAsync(connection);
        await using var pollVoteWriter = await pollVoteInserterFactory.CreateAsync(connection);

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
