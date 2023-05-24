namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterOrganizationalRelation> interOrganizationalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableInterOrganizationalRelation>
{
    public async Task<NodeCreator<EventuallyIdentifiableInterOrganizationalRelation>> CreateAsync(IDbConnection connection) =>
        new(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationInserterFactory.CreateAsync(connection)

            },
             await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
