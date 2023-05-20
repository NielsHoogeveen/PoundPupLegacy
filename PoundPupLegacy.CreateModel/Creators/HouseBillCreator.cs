namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseBillCreator(
    IDatabaseInserterFactory<HouseBill> houseBillInserterFactory,
    IDatabaseInserterFactory<Bill> billInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<HouseBill>
{
    public override async Task CreateAsync(IAsyncEnumerable<HouseBill> houseBills, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var billWriter = await billInserterFactory.CreateAsync(connection);
        await using var houseBillWriter = await houseBillInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var houseBill in houseBills) {
            await nodeWriter.InsertAsync(houseBill);
            await searchableWriter.InsertAsync(houseBill);
            await nameableWriter.InsertAsync(houseBill);
            await documentableWriter.InsertAsync(houseBill);
            await billWriter.InsertAsync(houseBill);
            await houseBillWriter.InsertAsync(houseBill);
            await WriteTerms(houseBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in houseBill.TenantNodes) {
                tenantNode.NodeId = houseBill.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
