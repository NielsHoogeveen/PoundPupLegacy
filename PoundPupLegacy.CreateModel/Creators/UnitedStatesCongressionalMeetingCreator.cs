namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesCongressionalMeetingCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableUnitedStatesCongressionalMeeting> unitedStatesCongressionalMeetingInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableUnitedStatesCongressionalMeeting>
{
    public async Task<IEntityCreator<EventuallyIdentifiableUnitedStatesCongressionalMeeting>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<EventuallyIdentifiableUnitedStatesCongressionalMeeting>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await unitedStatesCongressionalMeetingInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );

}
