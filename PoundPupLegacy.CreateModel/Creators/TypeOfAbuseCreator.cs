namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class TypeOfAbuseCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewTypeOfAbuse> typeOfAbuseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewTypeOfAbuse>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewTypeOfAbuse> typesOfAbuse, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var typeOfAbuseWriter = await typeOfAbuseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var typeOfAbuse in typesOfAbuse) {
            await nodeWriter.InsertAsync(typeOfAbuse);
            await searchableWriter.InsertAsync(typeOfAbuse);
            await nameableWriter.InsertAsync(typeOfAbuse);
            await typeOfAbuseWriter.InsertAsync(typeOfAbuse);
            await WriteTerms(typeOfAbuse, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in typeOfAbuse.TenantNodes) {
                tenantNode.NodeId = typeOfAbuse.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
