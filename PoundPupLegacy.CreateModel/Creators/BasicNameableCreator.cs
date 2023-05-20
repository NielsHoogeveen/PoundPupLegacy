namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class BasicNameableCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<BasicNameable> basicNameableInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<BasicNameable>
{
    public override async Task CreateAsync(IAsyncEnumerable<BasicNameable> basicNameables, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var basicNameableWriter = await basicNameableInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var basicNameable in basicNameables) {
            await nodeWriter.InsertAsync(basicNameable);
            await searchableWriter.InsertAsync(basicNameable);
            await nameableWriter.InsertAsync(basicNameable);
            await basicNameableWriter.InsertAsync(basicNameable);
            await WriteTerms(basicNameable, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in basicNameable.TenantNodes) {
                tenantNode.NodeId = basicNameable.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
