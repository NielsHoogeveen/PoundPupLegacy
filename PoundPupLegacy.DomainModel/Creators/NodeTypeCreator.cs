namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class NodeTypeCreatorFactory(
    IDatabaseInserterFactory<NodeTypeToAdd> nodeTypeInserterFactory
) : IEntityCreatorFactory<BasicNodeType>
{
    public async Task<IEntityCreator<BasicNodeType>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<BasicNodeType>(new() {
            await nodeTypeInserterFactory.CreateAsync(connection)
        });
}
