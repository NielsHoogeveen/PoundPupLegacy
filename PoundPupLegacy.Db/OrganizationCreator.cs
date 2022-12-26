using Npgsql;
using PoundPupLegacy.Db.Writers;
using PoundPupLegacy.Model;
namespace PoundPupLegacy.Db;

public class OrganizationCreator : IEntityCreator<Organization>
{
    public static void Create(IEnumerable<Organization> organizations, NpgsqlConnection connection)
    {

        using var nodeWriter = NodeWriter.Create(connection);
        using var documentableWriter = DocumentableWriter.Create(connection);
        using var locatableWriter = LocatableWriter.Create(connection);
        using var partyWriter = PartyWriter.Create(connection);
        using var organizationWriter = OrganizationWriter.Create(connection);

        foreach (var organization in organizations)
        {
            nodeWriter.Write(organization);
            documentableWriter.Write(organization);
            locatableWriter.Write(organization);
            partyWriter.Write(organization);
            organizationWriter.Write(organization);
        }
    }
}
