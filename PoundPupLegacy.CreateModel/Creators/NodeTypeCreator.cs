namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTypeCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNodeType> nodeTypeInserterFactory
) : IInsertingEntityCreatorFactory<BasicNodeType>
{
    public async Task<InsertingEntityCreator<BasicNodeType>> CreateAsync(IDbConnection connection) =>
        new(new() {
            await nodeTypeInserterFactory.CreateAsync(connection)
        });
}
