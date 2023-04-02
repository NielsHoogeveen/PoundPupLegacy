namespace PoundPupLegacy.CreateModel.Creators;

public class HouseBillCreator : IEntityCreator<HouseBill>
{
    public async Task CreateAsync(IAsyncEnumerable<HouseBill> houseBills, IDbConnection connection)
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
            await nodeWriter.InsertAsync(houseBill);
            await searchableWriter.InsertAsync(houseBill);
            await nameableWriter.InsertAsync(houseBill);
            await documentableWriter.InsertAsync(houseBill);
            await billWriter.InsertAsync(houseBill);
            await houseBillWriter.InsertAsync(houseBill);
            await EntityCreator.WriteTerms(houseBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in houseBill.TenantNodes) {
                tenantNode.NodeId = houseBill.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
