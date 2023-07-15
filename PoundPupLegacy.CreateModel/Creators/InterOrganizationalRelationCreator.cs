namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class InterOrganizationalRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterOrganizationalRelation.ToCreate.ForExistingParticipants> interOrganizationalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterOrganizationalRelation.ToCreate.ForExistingParticipants>
{
    public async Task<IEntityCreator<InterOrganizationalRelation.ToCreate.ForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterOrganizationalRelation.ToCreate.ForExistingParticipants>(
            new()
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationInserterFactory.CreateAsync(connection)

            },
             await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
