using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class DeportationCaseCreator : IEntityCreator<DeportationCase>
{
    public static void Create(IEnumerable<DeportationCase> deportationCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var deportationCaseWriter = DeportationCaseWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var deportationCase in deportationCases)
        {
            nodeWriter.Write(deportationCase);
            documentableWriter.Write(deportationCase);
            locatableWriter.Write(deportationCase);
            caseWriter.Write(deportationCase);
            deportationCaseWriter.Write(deportationCase);
            EntityCreator.WriteTerms(deportationCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
