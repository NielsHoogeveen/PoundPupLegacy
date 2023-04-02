namespace PoundPupLegacy.CreateModel.Creators;

public class WrongfulMedicationCaseCreator : IEntityCreator<WrongfulMedicationCase>
{
    public async Task CreateAsync(IAsyncEnumerable<WrongfulMedicationCase> wrongfulMedicationCases, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var caseWriter = await CaseInserter.CreateAsync(connection);
        await using var wrongfulMedicationCaseWriter = await WrongfulMedicationCaseInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var wrongfulMedicationCase in wrongfulMedicationCases) {
            await nodeWriter.InsertAsync(wrongfulMedicationCase);
            await searchableWriter.InsertAsync(wrongfulMedicationCase);
            await documentableWriter.InsertAsync(wrongfulMedicationCase);
            await locatableWriter.InsertAsync(wrongfulMedicationCase);
            await nameableWriter.InsertAsync(wrongfulMedicationCase);
            await caseWriter.InsertAsync(wrongfulMedicationCase);
            await wrongfulMedicationCaseWriter.InsertAsync(wrongfulMedicationCase);
            await EntityCreator.WriteTerms(wrongfulMedicationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulMedicationCase.TenantNodes) {
                tenantNode.NodeId = wrongfulMedicationCase.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
