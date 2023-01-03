using PoundPupLegacy.Db.Readers;

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
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var wrongfulMedicationCase in wrongfulMedicationCases)
        {
            nodeWriter.Write(wrongfulMedicationCase);
            documentableWriter.Write(wrongfulMedicationCase);
            locatableWriter.Write(wrongfulMedicationCase);
            caseWriter.Write(wrongfulMedicationCase);
            wrongfulMedicationCaseWriter.Write(wrongfulMedicationCase);
            EntityCreator.WriteTerms(wrongfulMedicationCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
