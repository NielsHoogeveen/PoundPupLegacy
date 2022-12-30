using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

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

        foreach (var coercedAdoptionCase in coercedAdoptionCases)
        {
            nodeWriter.Write(coercedAdoptionCase);
            documentableWriter.Write(coercedAdoptionCase);
            locatableWriter.Write(coercedAdoptionCase);
            caseWriter.Write(coercedAdoptionCase);
            coercedAdoptionCaseWriter.Write(coercedAdoptionCase);
        }
    }
}
