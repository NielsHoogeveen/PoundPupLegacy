using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class PoliticalEntityRelationTypeCreator : IEntityCreator<PoliticalEntityRelationType>
{
    public static void Create(IEnumerable<PoliticalEntityRelationType> politicalEntityRelationTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var politicalEntityRelationTypeWriter = PoliticalEntityRelationTypeWriter.Create(connection);
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var politicalEntityRelationType in politicalEntityRelationTypes)
        {
            nodeWriter.Write(politicalEntityRelationType);
            nameableWriter.Write(politicalEntityRelationType);
            politicalEntityRelationTypeWriter.Write(politicalEntityRelationType);
            EntityCreator.WriteTerms(politicalEntityRelationType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
