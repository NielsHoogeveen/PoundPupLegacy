namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HagueStatusCreator(
    IDatabaseInserterFactory<NewHagueStatus> hagueStatusInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory
) : EntityCreator<NewHagueStatus>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewHagueStatus> hagueStatuss, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var hagueStatusWriter = await hagueStatusInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var hagueStatus in hagueStatuss) {
            await nodeWriter.InsertAsync(hagueStatus);
            await searchableWriter.InsertAsync(hagueStatus);
            await nameableWriter.InsertAsync(hagueStatus);
            await hagueStatusWriter.InsertAsync(hagueStatus);
            await WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in hagueStatus.TenantNodes) {
                tenantNode.NodeId = hagueStatus.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
