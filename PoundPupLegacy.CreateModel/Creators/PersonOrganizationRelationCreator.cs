namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePersonOrganizationRelation> personOrganizationRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiablePersonOrganizationRelation>
{
    public async Task<NodeCreator<EventuallyIdentifiablePersonOrganizationRelation>> CreateAsync(IDbConnection connection) => 
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await personOrganizationRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
