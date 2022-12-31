namespace PoundPupLegacy.Db.Writers;

internal class PoliticalEntityRelationTypeWriter : IDatabaseWriter<PoliticalEntityRelationType>
{
    public static DatabaseWriter<PoliticalEntityRelationType> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<PoliticalEntityRelationType>(SingleIdWriter.CreateSingleIdCommand("political_entity_relation_type", connection));
    }
}
