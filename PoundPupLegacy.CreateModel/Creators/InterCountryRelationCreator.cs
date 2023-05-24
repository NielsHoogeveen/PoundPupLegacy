namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterCountryRelation> interCountryRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableInterCountryRelation>
{
    public async Task<NodeCreator<EventuallyIdentifiableInterCountryRelation>> CreateAsync(IDbConnection connection) =>
        new(
            new () 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interCountryRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
