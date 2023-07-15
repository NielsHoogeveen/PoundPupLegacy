namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class PersonOrganizationRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<PersonOrganizationRelation.ToCreate.ForExistingParticipants> personOrganizationRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<PersonOrganizationRelation.ToCreate.ForExistingParticipants>
{
    public async Task<IEntityCreator<PersonOrganizationRelation.ToCreate.ForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<PersonOrganizationRelation.ToCreate.ForExistingParticipants>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await personOrganizationRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
