namespace PoundPupLegacy.Db;

public class WrongfulMedicationCaseCreator : IEntityCreator<WrongfulMedicationCase>
{
    public static void Create(IEnumerable<WrongfulMedicationCase> wrongfulMedicationCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var wrongfulMedicationCaseWriter = WrongfulMedicationCaseWriter.Create(connection);

        foreach (var wrongfulMedicationCase in wrongfulMedicationCases)
        {
            nodeWriter.Write(wrongfulMedicationCase);
            documentableWriter.Write(wrongfulMedicationCase);
            locatableWriter.Write(wrongfulMedicationCase);
            caseWriter.Write(wrongfulMedicationCase);
            wrongfulMedicationCaseWriter.Write(wrongfulMedicationCase);
        }
    }
}
