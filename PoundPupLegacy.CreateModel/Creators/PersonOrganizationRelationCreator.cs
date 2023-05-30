namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<PersonOrganizationRelation.ToCreateForExistingParticipants> personOrganizationRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PersonOrganizationRelation.ToCreateForExistingParticipants>
{
    public async Task<IEntityCreator<PersonOrganizationRelation.ToCreateForExistingParticipants>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<PersonOrganizationRelation.ToCreateForExistingParticipants>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await personOrganizationRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
