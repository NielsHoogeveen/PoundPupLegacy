namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateBillCreator : IEntityCreator<SenateBill>
{
    public async Task CreateAsync(IAsyncEnumerable<SenateBill> senateBills, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var billWriter = await BillInserter.CreateAsync(connection);
        await using var senateBillWriter = await SenateBillInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);

        await foreach (var senateBill in senateBills) {
            await nodeWriter.InsertAsync(senateBill);
            await searchableWriter.InsertAsync(senateBill);
            await nameableWriter.InsertAsync(senateBill);
            await documentableWriter.InsertAsync(senateBill);
            await billWriter.InsertAsync(senateBill);
            await senateBillWriter.InsertAsync(senateBill);
            await EntityCreator.WriteTerms(senateBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in senateBill.TenantNodes) {
                tenantNode.NodeId = senateBill.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
