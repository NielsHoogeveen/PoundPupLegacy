namespace PoundPupLegacy.CreateModel.Creators;

public class VocabularyCreator : IEntityCreator<Vocabulary>
{
    public static async Task CreateAsync(IAsyncEnumerable<Vocabulary> vocabularies, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var vocabularyWriter = await VocabularyWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var vocabulary in vocabularies) {
            await nodeWriter.WriteAsync(vocabulary);
            await vocabularyWriter.WriteAsync(vocabulary);
            foreach (var tenantNode in vocabulary.TenantNodes) {
                tenantNode.NodeId = vocabulary.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }
        }
    }
}
