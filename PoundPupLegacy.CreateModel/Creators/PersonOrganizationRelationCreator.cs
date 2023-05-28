namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> personOrganizationRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants>
{
    public async Task<IEntityCreator<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await personOrganizationRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
