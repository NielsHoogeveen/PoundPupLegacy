namespace PoundPupLegacy.CreateModel.Creators;

public class MultiQuestionPollCreator : IEntityCreator<MultiQuestionPoll>
{
    public static async Task CreateAsync(IAsyncEnumerable<MultiQuestionPoll> polls, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var simpleTextNodeWriter = await SimpleTextNodeInserter.CreateAsync(connection);
        await using var pollWriter = await PollInserter.CreateAsync(connection);
        await using var multiQuesionPollWriter = await MultiQuestionPollInserter.CreateAsync(connection);
        await using var pollQuestionWriter = await PollQuestionInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var pollOptionWriter = await PollOptionInserter.CreateAsync(connection);
        await using var pollVoteWriter = await PollVoteInserter.CreateAsync(connection);
        await using var multiQuestionPollPollQuestionWriter = await MultiQuestionPollPollQuestionInserter.CreateAsync(connection);

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
