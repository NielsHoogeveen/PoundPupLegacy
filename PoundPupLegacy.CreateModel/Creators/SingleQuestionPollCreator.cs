namespace PoundPupLegacy.CreateModel.Creators;

public class SingleQuestionPollCreator : IEntityCreator<SingleQuestionPoll>
{
    public static async Task CreateAsync(IAsyncEnumerable<SingleQuestionPoll> polls, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var pollWriter = await PollWriter.CreateAsync(connection);
        await using var pollQuestionWriter = await PollQuestionWriter.CreateAsync(connection);
        await using var singleQuestionPollWriter = await SingleQuestionPollWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);
        await using var pollOptionWriter = await PollOptionWriter.CreateAsync(connection);
        await using var pollVoteWriter = await PollVoteWriter.CreateAsync(connection);

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
