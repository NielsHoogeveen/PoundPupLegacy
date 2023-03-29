namespace PoundPupLegacy.CreateModel.Creators;

public class HouseBillCreator : IEntityCreator<HouseBill>
{
    public static async Task CreateAsync(IAsyncEnumerable<HouseBill> houseBills, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var billWriter = await BillInserter.CreateAsync(connection);
        await using var houseBillWriter = await HouseBillInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);

        await foreach (var houseBill in houseBills) {
            await nodeWriter.WriteAsync(houseBill);
            await searchableWriter.WriteAsync(houseBill);
            await nameableWriter.WriteAsync(houseBill);
            await documentableWriter.WriteAsync(houseBill);
            await billWriter.WriteAsync(houseBill);
            await houseBillWriter.WriteAsync(houseBill);
            await EntityCreator.WriteTerms(houseBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in houseBill.TenantNodes) {
                tenantNode.NodeId = houseBill.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
