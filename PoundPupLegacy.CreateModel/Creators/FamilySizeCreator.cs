namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FamilySizeCreator(IDatabaseInserterFactory<NewFamilySize> familySizeInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewFamilySize>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewFamilySize> familySizes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var familySizeWriter = await familySizeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var familySize in familySizes) {
            await nodeWriter.InsertAsync(familySize);
            await searchableWriter.InsertAsync(familySize);
            await nameableWriter.InsertAsync(familySize);
            await familySizeWriter.InsertAsync(familySize);
            await WriteTerms(familySize, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in familySize.TenantNodes) {
                tenantNode.NodeId = familySize.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
