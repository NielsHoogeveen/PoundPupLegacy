using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class SenateBillCreator : IEntityCreator<SenateBill>
{
    public static async Task CreateAsync(IAsyncEnumerable<SenateBill> senateBills, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var billWriter = await BillWriter.CreateAsync(connection);
        await using var senateBillWriter = await SenateBillWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await (new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);

        await foreach (var senateBill in senateBills) {
            await nodeWriter.WriteAsync(senateBill);
            await searchableWriter.WriteAsync(senateBill);
            await nameableWriter.WriteAsync(senateBill);
            await documentableWriter.WriteAsync(senateBill);
            await billWriter.WriteAsync(senateBill);
            await senateBillWriter.WriteAsync(senateBill);
            await EntityCreator.WriteTerms(senateBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in senateBill.TenantNodes) {
                tenantNode.NodeId = senateBill.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
