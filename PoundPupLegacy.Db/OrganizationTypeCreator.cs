namespace PoundPupLegacy.Db;

public class OrganizationTypeCreator : IEntityCreator<OrganizationType>
{
    public static void Create(IEnumerable<OrganizationType> organizationTypes, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var nameableWriter = NameableWriter.Create(connection);
        using var organizationTypeWriter = OrganizationTypeWriter.Create(connection);

        foreach (var organizationType in organizationTypes)
        {
            nodeWriter.Write(organizationType);
            nameableWriter.Write(organizationType);
            organizationTypeWriter.Write(organizationType);
        }
    }
}
