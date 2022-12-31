namespace PoundPupLegacy.Db.Writers;

internal class InterOrganizationalRelationTypeWriter : IDatabaseWriter<InterOrganizationalRelationType>
{
    public static DatabaseWriter<InterOrganizationalRelationType> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<InterOrganizationalRelationType>(SingleIdWriter.CreateSingleIdCommand("inter_organizational_relation_type", connection));
    }
}
