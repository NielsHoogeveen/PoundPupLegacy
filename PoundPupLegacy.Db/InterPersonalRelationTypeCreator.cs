using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class InterPersonalRelationTypeCreator : IEntityCreator<InterPersonalRelationType>
{
    public static void Create(IEnumerable<InterPersonalRelationType> interPersonalRelationTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var interPersonalRelationTypeWriter = InterPersonalRelationTypeWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);


        foreach (var interPersonalRelationType in interPersonalRelationTypes)
        {
            nodeWriter.Write(interPersonalRelationType);
            nameableWriter.Write(interPersonalRelationType);
            interPersonalRelationTypeWriter.Write(interPersonalRelationType);
            EntityCreator.WriteTerms(interPersonalRelationType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
