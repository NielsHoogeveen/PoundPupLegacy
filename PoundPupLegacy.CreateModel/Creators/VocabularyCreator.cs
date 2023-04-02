namespace PoundPupLegacy.CreateModel.Creators;

public class VocabularyCreator : IEntityCreator<Vocabulary>
{
    public async Task CreateAsync(IAsyncEnumerable<Vocabulary> vocabularies, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var vocabularyWriter = await VocabularyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
