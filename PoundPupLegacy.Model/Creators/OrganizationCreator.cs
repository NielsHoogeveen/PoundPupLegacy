namespace PoundPupLegacy.CreateModel.Creators;

public class OrganizationCreator : IEntityCreator<Organization>
{
    public static async Task CreateAsync(IAsyncEnumerable<Organization> organizations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var partyWriter = await PartyWriter.CreateAsync(connection);
        await using var organizationWriter = await OrganizationWriter.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyWriter = await UnitedStatesPoliticalPartyWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await (new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);
        await using var organizationOrganizationTypeWriter = await OrganizationOrganizationTypeWriter.CreateAsync(connection);

        await foreach (var organization in organizations) {
            await nodeWriter.WriteAsync(organization);
            await searchableWriter.WriteAsync(organization);
            await documentableWriter.WriteAsync(organization);
            await locatableWriter.WriteAsync(organization);
            await nameableWriter.WriteAsync(organization);
            await partyWriter.WriteAsync(organization);
            await organizationWriter.WriteAsync(organization);
            if (organization is UnitedStatesPoliticalParty pp) {
                await unitedStatesPoliticalPartyWriter.WriteAsync(pp);
            }
            await EntityCreator.WriteTerms(organization, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in organization.TenantNodes) {
                tenantNode.NodeId = organization.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }
            foreach (var organizationOrganizationType in organization.OrganizationTypes) {
                organizationOrganizationType.OrganizationId = organization.Id;
                await organizationOrganizationTypeWriter.WriteAsync(organizationOrganizationType);
            }
        }
    }
}
