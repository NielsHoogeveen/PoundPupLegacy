namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicNameableTypeCreator(
    IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory,
    IDatabaseInserterFactory<NameableType> nameableTypeInserterFactory
) : EntityCreator<BasicNameableType>
{
    public override async Task CreateAsync(IAsyncEnumerable<BasicNameableType> nameableTypes, IDbConnection connection)
    {
        await using var nodeTypeWriter = await nodeTypeInserterFactory.CreateAsync(connection);
        await using var nameableTypeWriter = await nameableTypeInserterFactory.CreateAsync(connection);
        await foreach (var nameableType in nameableTypes) {
            await nodeTypeWriter.InsertAsync(nameableType);
            await nameableTypeWriter.InsertAsync(nameableType);
        }
    }
}
