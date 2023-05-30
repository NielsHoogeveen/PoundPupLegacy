namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<InterOrganizationalRelation.ToCreateForExistingParticipants> interOrganizationalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<InterOrganizationalRelation.ToCreateForExistingParticipants>
{
    public async Task<IEntityCreator<InterOrganizationalRelation.ToCreateForExistingParticipants>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<InterOrganizationalRelation.ToCreateForExistingParticipants>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationInserterFactory.CreateAsync(connection)

            },
             await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
