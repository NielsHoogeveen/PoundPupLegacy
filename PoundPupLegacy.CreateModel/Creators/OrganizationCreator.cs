namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class OrganizationCreator(
    IDatabaseInserterFactory<Node> nodeIntererFactory,
    IDatabaseInserterFactory<Searchable> searchableInserterFactory,
    IDatabaseInserterFactory<Documentable> documentableInserterFactory,
    IDatabaseInserterFactory<Locatable> locatableInserterFactory,
    IDatabaseInserterFactory<Nameable> nameableInserterFactory,
    IDatabaseInserterFactory<Party> partyInserterFactory,
    IDatabaseInserterFactory<Organization> organizationInserterFactory,
    IDatabaseInserterFactory<NewUnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserterFactory,
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
    IDatabaseInserterFactory<OrganizationOrganizationType> organizationOrganizationTypeInserterFactory
) : EntityCreator<EventuallyIdentifiableOrganization>
{
    public override async Task CreateAsync(IAsyncEnumerable<EventuallyIdentifiableOrganization> organizations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeIntererFactory.CreateAsync(connection);
        await using var searchableWriter = await searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await nameableInserterFactory.CreateAsync(connection);
        await using var partyWriter = await partyInserterFactory.CreateAsync(connection);
        await using var organizationWriter = await organizationInserterFactory.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyWriter = await unitedStatesPoliticalPartyInserterFactory.CreateAsync(connection);
        await using var termWriter = await termInserterFactory.CreateAsync(connection);
        await using var termReader = await termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);
        await using var organizationOrganizationTypeWriter = await organizationOrganizationTypeInserterFactory.CreateAsync(connection);

        await foreach (var organization in organizations) {
            await nodeWriter.InsertAsync(organization);
            await searchableWriter.InsertAsync(organization);
            await documentableWriter.InsertAsync(organization);
            await locatableWriter.InsertAsync(organization);
            await nameableWriter.InsertAsync(organization);
            await partyWriter.InsertAsync(organization);
            await organizationWriter.InsertAsync(organization);
            if (organization is NewUnitedStatesPoliticalParty pp) {
                await unitedStatesPoliticalPartyWriter.InsertAsync(pp);
            }
            await WriteTerms(organization, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);

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
