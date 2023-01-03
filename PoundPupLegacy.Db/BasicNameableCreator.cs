using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class BasicNameableCreator : IEntityCreator<BasicNameable>
{
    public static void Create(IEnumerable<BasicNameable> basicNameables, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var basicNameableWriter = BasicNameableWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);
        foreach (var basicNameable in basicNameables)
        {
            nodeWriter.Write(basicNameable);
            nameableWriter.Write(basicNameable);
            basicNameableWriter.Write(basicNameable);
            EntityCreator.WriteTerms(basicNameable, termWriter, termReader, termHierarchyWriter);
        }
    }
}
