namespace PoundPupLegacy.Db;

public class PoliticalEntityRelationTypeCreator : IEntityCreator<PoliticalEntityRelationType>
{
    public static void Create(IEnumerable<PoliticalEntityRelationType> politicalEntityRelationTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var politicalEntityRelationTypeWriter = PoliticalEntityRelationTypeWriter.Create(connection);

        foreach (var politicalEntityRelationType in politicalEntityRelationTypes)
        {
            nodeWriter.Write(politicalEntityRelationType);
            nameableWriter.Write(politicalEntityRelationType);
            politicalEntityRelationTypeWriter.Write(politicalEntityRelationType);
        }
    }
}
