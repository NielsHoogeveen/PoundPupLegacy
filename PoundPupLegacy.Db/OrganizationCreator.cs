using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class OrganizationCreator : IEntityCreator<Organization>
{
    public static async Task CreateAsync(IAsyncEnumerable<Organization> organizations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var partyWriter = await PartyWriter.CreateAsync(connection);
        await using var organizationWriter = await OrganizationWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var organization in organizations)
        {
            await nodeWriter.WriteAsync(organization);
            await documentableWriter.WriteAsync(organization);
            await locatableWriter.WriteAsync(organization);
            await partyWriter.WriteAsync(organization);
            await organizationWriter.WriteAsync(organization);
            await EntityCreator.WriteTerms(organization, termWriter, termReader, termHierarchyWriter);
        }
    }
}
