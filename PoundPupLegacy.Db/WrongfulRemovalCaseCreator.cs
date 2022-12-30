using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class WrongfulRemovalCaseCreator : IEntityCreator<WrongfulRemovalCase>
{
    public static void Create(IEnumerable<WrongfulRemovalCase> wrongfulRemovalCases, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var caseWriter = CaseWriter.Create(connection);
        using var wrongfulRemovalCaseWriter = WrongfulRemovalCaseWriter.Create(connection);

        foreach (var wrongfulRemovalCase in wrongfulRemovalCases)
        {
            nodeWriter.Write(wrongfulRemovalCase);
            documentableWriter.Write(wrongfulRemovalCase);
            locatableWriter.Write(wrongfulRemovalCase);
            caseWriter.Write(wrongfulRemovalCase);
            wrongfulRemovalCaseWriter.Write(wrongfulRemovalCase);
        }
    }
}
