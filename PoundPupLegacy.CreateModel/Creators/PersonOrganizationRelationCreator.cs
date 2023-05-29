namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants> personOrganizationRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants>
{
    public async Task<IEntityCreator<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await personOrganizationRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
