using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class WrongfulMedicationCaseCreator : IEntityCreator<WrongfulMedicationCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<WrongfulMedicationCase> wrongfulMedicationCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var wrongfulMedicationCaseWriter = await WrongfulMedicationCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var wrongfulMedicationCase in wrongfulMedicationCases)
        {
            await nodeWriter.WriteAsync(wrongfulMedicationCase);
            await documentableWriter.WriteAsync(wrongfulMedicationCase);
            await locatableWriter.WriteAsync(wrongfulMedicationCase);
            await caseWriter.WriteAsync(wrongfulMedicationCase);
            await wrongfulMedicationCaseWriter.WriteAsync(wrongfulMedicationCase);
            await EntityCreator.WriteTerms(wrongfulMedicationCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
