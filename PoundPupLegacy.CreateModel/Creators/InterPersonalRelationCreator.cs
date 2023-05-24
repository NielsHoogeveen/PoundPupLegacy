namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableInterPersonalRelation> interPersonalRelationInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableInterPersonalRelation>
{
    public async Task<NodeCreator<EventuallyIdentifiableInterPersonalRelation>> CreateAsync(IDbConnection connection) =>
        new(
            new() 
            {
                await nodeInserterFactory.CreateAsync(connection),
                await interPersonalRelationInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
