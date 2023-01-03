using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class CoercedAdoptionCaseCreator : IEntityCreator<CoercedAdoptionCase>
{
    public static void Create(IEnumerable<CoercedAdoptionCase> coercedAdoptionCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var coercedAdoptionCaseWriter = CoercedAdoptionCaseWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var coercedAdoptionCase in coercedAdoptionCases)
        {
            nodeWriter.Write(coercedAdoptionCase);
            documentableWriter.Write(coercedAdoptionCase);
            locatableWriter.Write(coercedAdoptionCase);
            caseWriter.Write(coercedAdoptionCase);
            coercedAdoptionCaseWriter.Write(coercedAdoptionCase);
            EntityCreator.WriteTerms(coercedAdoptionCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
