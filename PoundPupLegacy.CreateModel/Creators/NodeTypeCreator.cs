namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NodeTypeCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNodeType> nodeTypeInserterFactory
) : IEntityCreatorFactory<BasicNodeType>
{
    public async Task<IEntityCreator<BasicNodeType>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicNodeType>(new() {
            await nodeTypeInserterFactory.CreateAsync(connection)
        });
}
