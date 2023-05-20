namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class MultiQuestionPollCreator(
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
) : EntityCreator<MultiQuestionPoll>
{
    public override async Task CreateAsync(IAsyncEnumerable<MultiQuestionPoll> polls, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var pollWriter = await pollInserterFactory.CreateAsync(connection);
        await using var multiQuesionPollWriter = await multiQuestionPollInserterFactory.CreateAsync(connection);
        await using var pollQuestionWriter = await pollQuestionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var pollOptionWriter = await pollOptionInserterFactory.CreateAsync(connection);
        await using var pollVoteWriter = await pollVoteInserterFactory.CreateAsync(connection);
        await using var multiQuestionPollPollQuestionWriter = await multiQuestionPollPollQuestionInserterFactory.CreateAsync(connection);

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
