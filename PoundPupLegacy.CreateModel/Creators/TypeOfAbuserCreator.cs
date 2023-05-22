namespace PoundPupLegacy.CreateModel.Creators;

internal class TypeOfAbuserCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<TypeOfAbuser> typeOfAbuserInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewTypeOfAbuser>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewTypeOfAbuser> typesOfAbuser, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var typeOfAbuserWriter = await typeOfAbuserInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var typeOfAbuser in typesOfAbuser) {
            await nodeWriter.InsertAsync(typeOfAbuser);
            await searchableWriter.InsertAsync(typeOfAbuser);
            await nameableWriter.InsertAsync(typeOfAbuser);
            await typeOfAbuserWriter.InsertAsync(typeOfAbuser);
            await WriteTerms(typeOfAbuser, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in typeOfAbuser.TenantNodes) {
                tenantNode.NodeId = typeOfAbuser.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
