namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SubdivisionTypeCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<NewSubdivisionType> subdivisionTypeInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewSubdivisionType>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewSubdivisionType> subdivisionTypes, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var subdivisionTypeWriter = await subdivisionTypeInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var subdivisionType in subdivisionTypes) {
            await nodeWriter.InsertAsync(subdivisionType);
            await searchableWriter.InsertAsync(subdivisionType);
            await nameableWriter.InsertAsync(subdivisionType);
            await subdivisionTypeWriter.InsertAsync(subdivisionType);
            await WriteTerms(subdivisionType, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivisionType.TenantNodes) {
                tenantNode.NodeId = subdivisionType.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
