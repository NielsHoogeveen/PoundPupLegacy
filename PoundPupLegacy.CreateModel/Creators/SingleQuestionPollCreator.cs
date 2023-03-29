namespace PoundPupLegacy.CreateModel.Creators;

public class SingleQuestionPollCreator : IEntityCreator<SingleQuestionPoll>
{
    public static async Task CreateAsync(IAsyncEnumerable<SingleQuestionPoll> polls, NpgsqlConnection connection)
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
            await nodeWriter.WriteAsync(poll);
            await searchableWriter.WriteAsync(poll);
            await simpleTextNodeWriter.WriteAsync(poll);
            await pollWriter.WriteAsync(poll);
            await pollQuestionWriter.WriteAsync(poll);
            await singleQuestionPollWriter.WriteAsync(poll);
            foreach (var tenantNode in poll.TenantNodes) {
                tenantNode.NodeId = poll.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }
            foreach (var pollOption in poll.PollOptions) {
                pollOption.PollQuestionId = poll.Id;
                await pollOptionWriter.WriteAsync(pollOption);
            }
            foreach (var pollVote in poll.PollVotes) {
                pollVote.PollId = poll.Id;
                await pollVoteWriter.WriteAsync(pollVote);
            }

        }
    }
}
