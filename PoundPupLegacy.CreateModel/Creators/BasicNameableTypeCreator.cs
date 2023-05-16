namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicNameableTypeCreator : EntityCreator<BasicNameableType>
{
    private readonly IDatabaseInserterFactory<NodeType> _nodeTypeInserterFactory;
    private readonly IDatabaseInserterFactory<NameableType> _nameableTypeInserterFactory;
    public BasicNameableTypeCreator(
        IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory,
        IDatabaseInserterFactory<NameableType> nameableTypeInserterFactory
        )
    {
        _nodeTypeInserterFactory = nodeTypeInserterFactory;
        _nameableTypeInserterFactory = nameableTypeInserterFactory;

    }
    public override async Task CreateAsync(IAsyncEnumerable<BasicNameableType> nameableTypes, IDbConnection connection)
    {

        await using var nodeTypeWriter = await _nodeTypeInserterFactory.CreateAsync(connection);
        await using var nameableTypeWriter = await _nameableTypeInserterFactory.CreateAsync(connection);
        await foreach (var nameableType in nameableTypes) {
            await nodeTypeWriter.InsertAsync(nameableType);
            await nameableTypeWriter.InsertAsync(nameableType);
        }
    }
}
