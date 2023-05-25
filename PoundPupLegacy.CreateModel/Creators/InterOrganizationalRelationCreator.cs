namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterOrganizationalRelation> interOrganizationalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableInterOrganizationalRelation>
{
    public async Task<IEntityCreator<EventuallyIdentifiableInterOrganizationalRelation>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<EventuallyIdentifiableInterOrganizationalRelation>(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interOrganizationalRelationInserterFactory.CreateAsync(connection)

            },
             await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
