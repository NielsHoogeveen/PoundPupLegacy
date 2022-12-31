namespace PoundPupLegacy.Db.Writers;

internal class OrganizationTypeWriter : IDatabaseWriter<OrganizationType>
{
    public static DatabaseWriter<OrganizationType> Create(NpgsqlConnection connection)
    {
        return new SingleIdWriter<OrganizationType>(SingleIdWriter.CreateSingleIdCommand("organization_type", connection));
    }
}
