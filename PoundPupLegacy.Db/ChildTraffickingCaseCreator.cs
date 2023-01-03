using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class ChildTraffickingCaseCreator : IEntityCreator<ChildTraffickingCase>
{
    public static void Create(IEnumerable<ChildTraffickingCase> childTraffickingCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var childTraffickingCaseWriter = ChildTraffickingCaseWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var childTraffickingCase in childTraffickingCases)
        {
            nodeWriter.Write(childTraffickingCase);
            documentableWriter.Write(childTraffickingCase);
            locatableWriter.Write(childTraffickingCase);
            caseWriter.Write(childTraffickingCase);
            childTraffickingCaseWriter.Write(childTraffickingCase);
            EntityCreator.WriteTerms(childTraffickingCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
