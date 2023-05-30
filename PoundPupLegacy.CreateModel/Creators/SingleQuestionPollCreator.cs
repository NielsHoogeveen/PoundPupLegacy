namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreatorFactory(
    IEntityCreatorFactory<PollQuestionToCreate> pollQuestionCreatorFactory,
    IDatabaseInserterFactory<PollToCreate> pollInserterFactory,
    IDatabaseInserterFactory<SingleQuestionPoll.ToCreate> singleQuestionPollInserterFactory
) : IEntityCreatorFactory<SingleQuestionPoll.ToCreate>
{
    public async Task<IEntityCreator<SingleQuestionPoll.ToCreate>> CreateAsync(IDbConnection connection) =>
        new SingleQuestionPollCreator(
            await pollQuestionCreatorFactory.CreateAsync(connection),
            await pollInserterFactory.CreateAsync(connection),
            await singleQuestionPollInserterFactory.CreateAsync(connection)
        );
}

public class SingleQuestionPollCreator(
    IEntityCreator<PollQuestionToCreate> pollQuestionCreator,
    IDatabaseInserter<PollToCreate> pollInserter,
    IDatabaseInserter<SingleQuestionPoll.ToCreate> singleQuestionPollInserter
) : EntityCreator<SingleQuestionPoll.ToCreate>
{
    public override async Task ProcessAsync(SingleQuestionPoll.ToCreate element)
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