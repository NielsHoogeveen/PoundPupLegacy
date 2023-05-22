namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class WrongfulRemovalCaseCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<WrongfulRemovalCase> wrongfulRemovalCaseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewWrongfulRemovalCase>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewWrongfulRemovalCase> wrongfulRemovalCases, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var wrongfulRemovalCaseWriter = await wrongfulRemovalCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var wrongfulRemovalCase in wrongfulRemovalCases) {
            await nodeWriter.InsertAsync(wrongfulRemovalCase);
            await searchableWriter.InsertAsync(wrongfulRemovalCase);
            await documentableWriter.InsertAsync(wrongfulRemovalCase);
            await locatableWriter.InsertAsync(wrongfulRemovalCase);
            await nameableWriter.InsertAsync(wrongfulRemovalCase);
            await caseWriter.InsertAsync(wrongfulRemovalCase);
            await wrongfulRemovalCaseWriter.InsertAsync(wrongfulRemovalCase);
            await WriteTerms(wrongfulRemovalCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulRemovalCase.TenantNodes) {
                tenantNode.NodeId = wrongfulRemovalCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
