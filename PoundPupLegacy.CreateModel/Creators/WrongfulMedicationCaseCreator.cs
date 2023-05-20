namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class WrongfulMedicationCaseCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Case> caseInserterFactory,
    IDatabaseInserterFactory<WrongfulMedicationCase> wrongfulMedicationCaseInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<WrongfulMedicationCase>
{
    public override async Task CreateAsync(IAsyncEnumerable<WrongfulMedicationCase> wrongfulMedicationCases, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var caseWriter = await caseInserterFactory.CreateAsync(connection);
        await using var wrongfulMedicationCaseWriter = await wrongfulMedicationCaseInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var wrongfulMedicationCase in wrongfulMedicationCases) {
            await nodeWriter.InsertAsync(wrongfulMedicationCase);
            await searchableWriter.InsertAsync(wrongfulMedicationCase);
            await documentableWriter.InsertAsync(wrongfulMedicationCase);
            await locatableWriter.InsertAsync(wrongfulMedicationCase);
            await nameableWriter.InsertAsync(wrongfulMedicationCase);
            await caseWriter.InsertAsync(wrongfulMedicationCase);
            await wrongfulMedicationCaseWriter.InsertAsync(wrongfulMedicationCase);
            await WriteTerms(wrongfulMedicationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulMedicationCase.TenantNodes) {
                tenantNode.NodeId = wrongfulMedicationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
