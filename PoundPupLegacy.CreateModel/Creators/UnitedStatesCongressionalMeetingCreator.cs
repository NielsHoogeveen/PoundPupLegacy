namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class UnitedStatesCongressionalMeetingCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate> unitedStatesCongressionalMeetingInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate>
{
    public async Task<IEntityCreator<UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<UnitedStatesCongressionalMeeting.UnitedStatesCongressionalMeetingToCreate>(
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
