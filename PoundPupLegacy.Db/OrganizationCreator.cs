using PoundPupLegacy.Db.Readers;

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
        using var termWriter = TermWriter.Create(connection);
        using var termReader = TermReader.Create(connection);
        using var termHierarchyWriter = TermHierarchyWriter.Create(connection);

        foreach (var organization in organizations)
        {
            nodeWriter.Write(organization);
            documentableWriter.Write(organization);
            locatableWriter.Write(organization);
            partyWriter.Write(organization);
            organizationWriter.Write(organization);
            EntityCreator.WriteTerms(organization, termWriter, termReader, termHierarchyWriter);
        }
    }
}
