namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTypeCreatorFactory(
    IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory
) : IInsertingEntityCreatorFactory<BasicNodeType>
{
    public async Task<InsertingEntityCreator<BasicNodeType>> CreateAsync(IDbConnection connection) =>
        new(new() {
            await nodeTypeInserterFactory.CreateAsync(connection)
        });
}
