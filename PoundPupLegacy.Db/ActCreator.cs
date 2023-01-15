using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class ActCreator : IEntityCreator<Act>
{
    public static async Task CreateAsync(IAsyncEnumerable<Act> acts, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var actWriter = await ActWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);

        await foreach (var act in acts)
        {
            await nodeWriter.WriteAsync(act);
            await nameableWriter.WriteAsync(act);
            await documentableWriter.WriteAsync(act);
            await actWriter.WriteAsync(act);
            await EntityCreator.WriteTerms(act, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in act.TenantNodes)
            {
                tenantNode.NodeId = act.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
