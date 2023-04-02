namespace PoundPupLegacy.CreateModel.Creators;

public class OrganizationCreator : IEntityCreator<Organization>
{
    public async Task CreateAsync(IAsyncEnumerable<Organization> organizations, IDbConnection connection)
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
            await nodeWriter.InsertAsync(organization);
            await searchableWriter.InsertAsync(organization);
            await documentableWriter.InsertAsync(organization);
            await locatableWriter.InsertAsync(organization);
            await nameableWriter.InsertAsync(organization);
            await partyWriter.InsertAsync(organization);
            await organizationWriter.InsertAsync(organization);
            if (organization is UnitedStatesPoliticalParty pp) {
                await unitedStatesPoliticalPartyWriter.InsertAsync(pp);
            }
            await EntityCreator.WriteTerms(organization, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

            foreach (var tenantNode in organization.TenantNodes) {
                tenantNode.NodeId = organization.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
            foreach (var organizationOrganizationType in organization.OrganizationTypes) {
                organizationOrganizationType.OrganizationId = organization.Id;
                await organizationOrganizationTypeWriter.InsertAsync(organizationOrganizationType);
            }
        }
    }
}
