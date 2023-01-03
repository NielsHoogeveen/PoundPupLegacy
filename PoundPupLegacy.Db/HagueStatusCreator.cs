using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class HagueStatusCreator : IEntityCreator<HagueStatus>
{
    public static void Create(IEnumerable<HagueStatus> hagueStatuss, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var hagueStatusWriter = HagueStatusWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var hagueStatus in hagueStatuss)
        {
            nodeWriter.Write(hagueStatus);
            nameableWriter.Write(hagueStatus);
            hagueStatusWriter.Write(hagueStatus);
            EntityCreator.WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter);
        }
    }
}
