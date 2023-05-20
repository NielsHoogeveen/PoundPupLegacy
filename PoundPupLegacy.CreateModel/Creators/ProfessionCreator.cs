namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ProfessionCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Profession> professionInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<Profession>
{
    public override async Task CreateAsync(IAsyncEnumerable<Profession> professions, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var professionWriter = await professionInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var profession in professions) {
            await nodeWriter.InsertAsync(profession);
            await searchableWriter.InsertAsync(profession);
            await nameableWriter.InsertAsync(profession);
            await professionWriter.InsertAsync(profession);
            await WriteTerms(profession, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in profession.TenantNodes) {
                tenantNode.NodeId = profession.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
