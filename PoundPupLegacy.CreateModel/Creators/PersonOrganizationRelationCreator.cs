namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiablePersonOrganizationRelation> personOrganizationRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiablePersonOrganizationRelation>
{
    public async Task<IEntityCreator<EventuallyIdentifiablePersonOrganizationRelation>> CreateAsync(IDbConnection connection) => 
        new NodeCreator<EventuallyIdentifiablePersonOrganizationRelation>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await personOrganizationRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
