namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreator : IEntityCreator<SingleQuestionPoll>
{
    public async Task CreateAsync(IAsyncEnumerable<SingleQuestionPoll> polls, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var pollWriter = await PollInserter.CreateAsync(connection);
        await using var pollQuestionWriter = await PollQuestionInserter.CreateAsync(connection);
        await using var singleQuestionPollWriter = await SingleQuestionPollInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var pollOptionWriter = await PollOptionInserter.CreateAsync(connection);
        await using var pollVoteWriter = await PollVoteInserter.CreateAsync(connection);

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
