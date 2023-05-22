namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class VocabularyCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<NewVocabulary> vocabularyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewVocabulary>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewVocabulary> vocabularies, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var vocabularyWriter = await vocabularyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
