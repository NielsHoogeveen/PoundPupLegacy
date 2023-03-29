namespace PoundPupLegacy.CreateModel.Creators;

public class OrganizationCreator : IEntityCreator<Organization>
{
    public static async Task CreateAsync(IAsyncEnumerable<Organization> organizations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var locatableWriter = await LocatableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var partyWriter = await PartyInserter.CreateAsync(connection);
        await using var organizationWriter = await OrganizationInserter.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyWriter = await UnitedStatesPoliticalPartyInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);
        await using var organizationOrganizationTypeWriter = await OrganizationOrganizationTypeInserter.CreateAsync(connection);

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
