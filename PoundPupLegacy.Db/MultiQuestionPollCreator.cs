namespace PoundPupLegacy.Db;

public class MultiQuestionPollCreator : IEntityCreator<MultiQuestionPoll>
{
    public static async Task CreateAsync(IAsyncEnumerable<MultiQuestionPoll> polls, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeWriter.CreateAsync(connection);
        await using var pollWriter = await PollWriter.CreateAsync(connection);
        await using var multiQuesionPollWriter = await MultiQuestionPollWriter.CreateAsync(connection);
        await using var pollQuestionWriter = await PollQuestionWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);
        await using var pollOptionWriter = await PollOptionWriter.CreateAsync(connection);
        await using var pollVoteWriter = await PollVoteWriter.CreateAsync(connection);
        await using var multiQuestionPollPollQuestionWriter = await MultiQuestionPollPollQuestionWriter.CreateAsync(connection);

        await foreach (var poll in polls) {
            await nodeWriter.WriteAsync(poll);
            await searchableWriter.WriteAsync(poll);
            await simpleTextNodeWriter.WriteAsync(poll);
            await pollWriter.WriteAsync(poll);
            await multiQuesionPollWriter.WriteAsync(poll);
            foreach (var (question, index) in poll.PollQuestions.Select((q, i) => (q, i))) {
                await nodeWriter.WriteAsync(question);
                await searchableWriter.WriteAsync(question);
                await simpleTextNodeWriter.WriteAsync(question);
                await pollQuestionWriter.WriteAsync(question);

                var pollQuestions = new MultiQuestionPollPollQuestion {
                    MultiQuestionPollId = poll.Id!.Value,
                    PollQuestionId = question.Id!.Value,
                    Delta = index
                };
                await multiQuestionPollPollQuestionWriter.WriteAsync(pollQuestions);
                foreach (var pollOption in question.PollOptions) {
                    pollOption.PollQuestionId = question.Id;
                    await pollOptionWriter.WriteAsync(pollOption);
                }
                foreach (var pollVote in question.PollVotes) {
                    pollVote.PollId = question.Id;
                    await pollVoteWriter.WriteAsync(pollVote);
                }

                foreach (var tenantNode in question.TenantNodes) {
                    tenantNode.NodeId = question.Id;
                    await tenantNodeWriter.WriteAsync(tenantNode);
                }

            }

            foreach (var tenantNode in poll.TenantNodes) {
                tenantNode.NodeId = poll.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
