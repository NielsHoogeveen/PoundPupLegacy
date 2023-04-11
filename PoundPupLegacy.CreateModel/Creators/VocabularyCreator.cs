namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class VocabularyCreator : EntityCreator<Vocabulary>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Vocabulary> _vocabularyInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public VocabularyCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Vocabulary> vocabularyInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _vocabularyInserterFactory = vocabularyInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Vocabulary> vocabularies, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var vocabularyWriter = await _vocabularyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var vocabulary in vocabularies) {
            await nodeWriter.InsertAsync(vocabulary);
            await vocabularyWriter.InsertAsync(vocabulary);
            foreach (var tenantNode in vocabulary.TenantNodes) {
                tenantNode.NodeId = vocabulary.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
