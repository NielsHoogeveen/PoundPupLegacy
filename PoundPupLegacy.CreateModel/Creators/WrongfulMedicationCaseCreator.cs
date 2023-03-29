namespace PoundPupLegacy.CreateModel.Creators;

public class WrongfulMedicationCaseCreator : IEntityCreator<WrongfulMedicationCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<WrongfulMedicationCase> wrongfulMedicationCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var wrongfulMedicationCaseWriter = await WrongfulMedicationCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var wrongfulMedicationCase in wrongfulMedicationCases) {
            await nodeWriter.WriteAsync(wrongfulMedicationCase);
            await searchableWriter.WriteAsync(wrongfulMedicationCase);
            await documentableWriter.WriteAsync(wrongfulMedicationCase);
            await locatableWriter.WriteAsync(wrongfulMedicationCase);
            await nameableWriter.WriteAsync(wrongfulMedicationCase);
            await caseWriter.WriteAsync(wrongfulMedicationCase);
            await wrongfulMedicationCaseWriter.WriteAsync(wrongfulMedicationCase);
            await EntityCreator.WriteTerms(wrongfulMedicationCase, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in wrongfulMedicationCase.TenantNodes) {
                tenantNode.NodeId = wrongfulMedicationCase.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
