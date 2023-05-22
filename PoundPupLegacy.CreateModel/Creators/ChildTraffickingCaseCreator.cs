namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class ChildTraffickingCaseCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<NewChildTraffickingCase> childTraffickingCaseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewChildTraffickingCase>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewChildTraffickingCase> childTraffickingCases, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var childTraffickingCaseWriter = await childTraffickingCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var childTraffickingCase in childTraffickingCases) {
            await nodeWriter.InsertAsync(childTraffickingCase);
            await searchableWriter.InsertAsync(childTraffickingCase);
            await documentableWriter.InsertAsync(childTraffickingCase);
            await locatableWriter.InsertAsync(childTraffickingCase);
            await nameableWriter.InsertAsync(childTraffickingCase);
            await caseWriter.InsertAsync(childTraffickingCase);
            await childTraffickingCaseWriter.InsertAsync(childTraffickingCase);
            await WriteTerms(childTraffickingCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in childTraffickingCase.TenantNodes) {
                tenantNode.NodeId = childTraffickingCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
