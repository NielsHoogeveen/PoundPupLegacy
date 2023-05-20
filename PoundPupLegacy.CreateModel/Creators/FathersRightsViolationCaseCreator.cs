namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class FathersRightsViolationCaseCreator(
    IDatabaseInserterFactory<FathersRightsViolationCase> fathersRightsViolationCaseInserterFactory,
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<FathersRightsViolationCase>
{
    public override async Task CreateAsync(IAsyncEnumerable<FathersRightsViolationCase> fathersRightsViolationCases, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var fathersRightsViolationCaseWriter = await fathersRightsViolationCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var fathersRightsViolationCase in fathersRightsViolationCases) {
            await nodeWriter.InsertAsync(fathersRightsViolationCase);
            await searchableWriter.InsertAsync(fathersRightsViolationCase);
            await documentableWriter.InsertAsync(fathersRightsViolationCase);
            await locatableWriter.InsertAsync(fathersRightsViolationCase);
            await nameableWriter.InsertAsync(fathersRightsViolationCase);
            await caseWriter.InsertAsync(fathersRightsViolationCase);
            await fathersRightsViolationCaseWriter.InsertAsync(fathersRightsViolationCase);
            await WriteTerms(fathersRightsViolationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in fathersRightsViolationCase.TenantNodes) {
                tenantNode.NodeId = fathersRightsViolationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
