namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class NameableTypeCreator : EntityCreator<NameableType>
{
    private readonly IDatabaseInserterFactory<NodeType> _nodeTypeInserterFactory;
    private readonly IDatabaseInserterFactory<NameableType> _nameableTypeInserterFactory;
    public NameableTypeCreator(
        IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory,
        IDatabaseInserterFactory<NameableType> nameableTypeInserterFactory
        )
    {
        _nodeTypeInserterFactory = nodeTypeInserterFactory;
        _nameableTypeInserterFactory = nameableTypeInserterFactory;

    }
    public override async Task CreateAsync(IAsyncEnumerable<NameableType> caseTypes, IDbConnection connection)
    {

        await using var nodeTypeWriter = await _nodeTypeInserterFactory.CreateAsync(connection);
        await using var nameableTypeWriter = await _nameableTypeInserterFactory.CreateAsync(connection);
    }
}
