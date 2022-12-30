using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class FathersRightsViolationCaseCreator : IEntityCreator<FathersRightsViolationCase>
{
    public static void Create(IEnumerable<FathersRightsViolationCase> fathersRightsViolationCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var fathersRightsViolationCaseWriter = FathersRightsViolationCaseWriter.Create(connection);

        foreach (var fathersRightsViolationCase in fathersRightsViolationCases)
        {
            nodeWriter.Write(fathersRightsViolationCase);
            documentableWriter.Write(fathersRightsViolationCase);
            locatableWriter.Write(fathersRightsViolationCase);
            caseWriter.Write(fathersRightsViolationCase);
            fathersRightsViolationCaseWriter.Write(fathersRightsViolationCase);
        }
    }
}
