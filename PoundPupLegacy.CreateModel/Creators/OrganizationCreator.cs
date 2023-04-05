namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class OrganizationCreator : EntityCreator<Organization>
{
    private readonly IDatabaseInserterFactory<Node> _nodeIntererFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<Locatable> _locatableInserterFactory;
    private readonly IDatabaseInserterFactory<Nameable> _nameableInserterFactory;
    private readonly IDatabaseInserterFactory<Party> _partyInserterFactory;
    private readonly IDatabaseInserterFactory<Organization> _organizationInserterFactory;
    private readonly IDatabaseInserterFactory<UnitedStatesPoliticalParty> _unitedStatesPoliticalPartyInserterFactory;
    private readonly IDatabaseInserterFactory<Term> _termInserterFactory;
    private readonly IDatabaseReaderFactory<TermReaderByName> _termReaderFactory;
    private readonly IDatabaseInserterFactory<TermHierarchy> _termHierarchyInserterFactory;
    private readonly IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> _vocabularyIdReaderFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IDatabaseInserterFactory<OrganizationOrganizationType> _organizationOrganizationTypeInserterFactory;
    //Add ctor
    public OrganizationCreator(
        IDatabaseInserterFactory<Node> nodeIntererFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<Locatable> locatableInserterFactory,
        IDatabaseInserterFactory<Nameable> nameableInserterFactory,
        IDatabaseInserterFactory<Party> partyInserterFactory,
        IDatabaseInserterFactory<Organization> organizationInserterFactory,
        IDatabaseInserterFactory<UnitedStatesPoliticalParty> unitedStatesPoliticalPartyInserterFactory,
        IDatabaseInserterFactory<Term> termInserterFactory,
        IDatabaseReaderFactory<TermReaderByName> termReaderFactory,
        IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
        IDatabaseReaderFactory<VocabularyIdReaderByOwnerAndName> vocabularyIdReaderFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IDatabaseInserterFactory<OrganizationOrganizationType> organizationOrganizationTypeInserterFactory
    )
    {
        _nodeIntererFactory = nodeIntererFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _locatableInserterFactory = locatableInserterFactory;
        _nameableInserterFactory = nameableInserterFactory;
        _partyInserterFactory = partyInserterFactory;
        _organizationInserterFactory = organizationInserterFactory;
        _unitedStatesPoliticalPartyInserterFactory = unitedStatesPoliticalPartyInserterFactory;
        _termInserterFactory = termInserterFactory;
        _termReaderFactory = termReaderFactory;
        _termHierarchyInserterFactory = termHierarchyInserterFactory;
        _vocabularyIdReaderFactory = vocabularyIdReaderFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _organizationOrganizationTypeInserterFactory = organizationOrganizationTypeInserterFactory;
    }


    public override async Task CreateAsync(IAsyncEnumerable<Organization> organizations, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeIntererFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var locatableWriter = await _locatableInserterFactory.CreateAsync(connection);
        await using var nameableWriter = await _nameableInserterFactory.CreateAsync(connection);
        await using var partyWriter = await _partyInserterFactory.CreateAsync(connection);
        await using var organizationWriter = await _organizationInserterFactory.CreateAsync(connection);
        await using var unitedStatesPoliticalPartyWriter = await _unitedStatesPoliticalPartyInserterFactory.CreateAsync(connection);
        await using var termWriter = await _termInserterFactory.CreateAsync(connection);
        await using var termReader = await _termReaderFactory.CreateAsync(connection);
        await using var termHierarchyWriter = await _termHierarchyInserterFactory.CreateAsync(connection);
        await using var vocabularyIdReader = await _vocabularyIdReaderFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);
        await using var organizationOrganizationTypeWriter = await _organizationOrganizationTypeInserterFactory.CreateAsync(connection);

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
