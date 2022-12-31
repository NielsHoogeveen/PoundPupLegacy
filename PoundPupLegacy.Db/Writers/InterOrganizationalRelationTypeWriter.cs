namespace PoundPupLegacy.Db.Writers;

internal class InterPersonalRelationTypeWriter : IDatabaseWriter<InterPersonalRelationType>
{
    public static DatabaseWriter<InterPersonalRelationType> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<InterPersonalRelationType>(SingleIdWriter.CreateSingleIdCommand("inter_personal_relation_type", connection));
    }
}
