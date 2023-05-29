namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterOrganizationalRelation.InterOrganizationalRelationToCreateForExistingParticipants> interOrganizationalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterOrganizationalRelation.InterOrganizationalRelationToCreateForExistingParticipants>
{
    public async Task<IEntityCreator<InterOrganizationalRelation.InterOrganizationalRelationToCreateForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterOrganizationalRelation.InterOrganizationalRelationToCreateForExistingParticipants>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationInserterFactory.CreateAsync(connection)

            },
             await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
