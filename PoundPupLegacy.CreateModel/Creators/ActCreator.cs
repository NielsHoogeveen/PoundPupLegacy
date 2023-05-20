namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ActCreator(
    IDatabaseInserterFactory<Act> actInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory
) : EntityCreator<Act>
{
    public override async Task CreateAsync(IAsyncEnumerable<Act> acts, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var actWriter = await actInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);

        await foreach (var act in acts) {
            await nodeWriter.InsertAsync(act);
            await searchableWriter.InsertAsync(act);
            await nameableWriter.InsertAsync(act);
            await documentableWriter.InsertAsync(act);
            await actWriter.InsertAsync(act);
            await WriteTerms(act, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in act.TenantNodes) {
                tenantNode.NodeId = act.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
