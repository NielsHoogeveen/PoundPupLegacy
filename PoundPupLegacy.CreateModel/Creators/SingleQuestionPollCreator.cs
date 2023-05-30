namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreatorFactory(
    IEntityCreatorFactory<PollQuestionToCreate> pollQuestionCreatorFactory,
    IDatabaseInserterFactory<PollToCreate> pollInserterFactory,
    IDatabaseInserterFactory<SingleQuestionPoll.SingleQuestionPollToCreate> singleQuestionPollInserterFactory
) : IEntityCreatorFactory<SingleQuestionPoll.SingleQuestionPollToCreate>
{
    public async Task<IEntityCreator<SingleQuestionPoll.SingleQuestionPollToCreate>> CreateAsync(IDbConnection connection) =>
        new SingleQuestionPollCreator(
            await pollQuestionCreatorFactory.CreateAsync(connection),
            await pollInserterFactory.CreateAsync(connection),
            await singleQuestionPollInserterFactory.CreateAsync(connection)
        );
}

public class SingleQuestionPollCreator(
    IEntityCreator<PollQuestionToCreate> pollQuestionCreator,
    IDatabaseInserter<PollToCreate> pollInserter,
    IDatabaseInserter<SingleQuestionPoll.SingleQuestionPollToCreate> singleQuestionPollInserter
) : EntityCreator<SingleQuestionPoll.SingleQuestionPollToCreate>
{
    public override async Task ProcessAsync(SingleQuestionPoll.SingleQuestionPollToCreate element)
    {
        await base.ProcessAsync(element);
        await pollQuestionCreator.CreateAsync(element);
        await pollInserter.InsertAsync(element);
        await singleQuestionPollInserter.InsertAsync(element);

    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await pollInserter.DisposeAsync();
        await singleQuestionPollInserter.DisposeAsync();
        await pollQuestionCreator.DisposeAsync();
    }
}