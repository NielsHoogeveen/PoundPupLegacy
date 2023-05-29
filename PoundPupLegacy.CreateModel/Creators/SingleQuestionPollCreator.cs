﻿namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SingleQuestionPollCreatorFactory(
    IEntityCreatorFactory<PollQuestion> pollQuestionCreatorFactory,
    IDatabaseInserterFactory<PollToCreate> pollInserterFactory,
    IDatabaseInserterFactory<SingleQuestionPoll.SingleQuestionPollToCreate> singleQuestionPollInserterFactory
) : IEntityCreatorFactory<SingleQuestionPoll.SingleQuestionPollToCreate>
{
    public async Task<IEntityCreator<EventuallyIdentifiableSingleQuestionPoll>> CreateAsync(IDbConnection connection) =>
        new SingleQuestionPollCreator(
            await pollQuestionCreatorFactory.CreateAsync(connection),
            await pollInserterFactory.CreateAsync(connection),
            await singleQuestionPollInserterFactory.CreateAsync(connection)
        );
}

public class SingleQuestionPollCreator(
    IEntityCreator<EventuallyIdentifiablePollQuestion> pollQuestionCreator,
    IDatabaseInserter<PollToCreate> pollInserter,
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