namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateBillCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Bill> billInserterFactory,
    IDatabaseInserterFactory<SenateBill> senateBillInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory
) : EntityCreator<SenateBill>
{
    public override async Task CreateAsync(IAsyncEnumerable<SenateBill> senateBills, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var billWriter = await billInserterFactory.CreateAsync(connection);
        await using var senateBillWriter = await senateBillInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var senateBill in senateBills) {
            await nodeWriter.InsertAsync(senateBill);
            await searchableWriter.InsertAsync(senateBill);
            await nameableWriter.InsertAsync(senateBill);
            await documentableWriter.InsertAsync(senateBill);
            await billWriter.InsertAsync(senateBill);
            await senateBillWriter.InsertAsync(senateBill);
            await WriteTerms(senateBill, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in senateBill.TenantNodes) {
                tenantNode.NodeId = senateBill.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
