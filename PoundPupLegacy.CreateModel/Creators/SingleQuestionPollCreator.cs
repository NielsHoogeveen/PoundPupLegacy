namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreatorFactory(
    IEntityCreatorFactory<EventuallyIdentifiablePollQuestion> pollQuestionCreatorFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePoll> pollInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSingleQuestionPoll> singleQuestionPollInserterFactory
) : IEntityCreatorFactory<EventuallyIdentifiableSingleQuestionPoll>
{
    public async Task<EntityCreator<EventuallyIdentifiableSingleQuestionPoll>> CreateAsync(IDbConnection connection) =>
        new SingleQuestionPollCreator(
            await pollQuestionCreatorFactory.CreateAsync(connection),
            await pollInserterFactory.CreateAsync(connection),
            await singleQuestionPollInserterFactory.CreateAsync(connection)
        );
}

public class SingleQuestionPollCreator(
    IEntityCreator<EventuallyIdentifiablePollQuestion> pollQuestionCreator,
    IDatabaseInserter<EventuallyIdentifiablePoll> pollInserter,
    IDatabaseInserter<EventuallyIdentifiableSingleQuestionPoll> singleQuestionPollInserter
) : EntityCreator<EventuallyIdentifiableSingleQuestionPoll>
{
    public override async Task ProcessAsync(EventuallyIdentifiableSingleQuestionPoll element)
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