namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class UnitedStatesCongressionalMeetingCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<UnitedStatesCongressionalMeeting.ToCreate> unitedStatesCongressionalMeetingInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory
) : IEntityCreatorFactory<UnitedStatesCongressionalMeeting.ToCreate>
{
    public async Task<IEntityCreator<UnitedStatesCongressionalMeeting.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NameableCreator<UnitedStatesCongressionalMeeting.ToCreate>(
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
