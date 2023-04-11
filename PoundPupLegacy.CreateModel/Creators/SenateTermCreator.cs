namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenateTermCreator : EntityCreator<SenateTerm>
{

    private readonly IDatabaseInserterFactory<SenateTerm> _senateTermInserterFactory;
    private readonly IDatabaseInserterFactory<CongressionalTerm> _congressionalTermInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IEntityCreator<CongressionalTermPoliticalPartyAffiliation> _congressionalTermPoliticalPartyAffiliationCreator;
    public SenateTermCreator(
        IDatabaseInserterFactory<SenateTerm> senateTermInserterFactory,
        IDatabaseInserterFactory<CongressionalTerm> congressionalTermInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IEntityCreator<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreator
    )
    {
        _senateTermInserterFactory = senateTermInserterFactory;
        _congressionalTermInserterFactory = congressionalTermInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _congressionalTermPoliticalPartyAffiliationCreator = congressionalTermPoliticalPartyAffiliationCreator;
    }
    public override async Task CreateAsync(IAsyncEnumerable<SenateTerm> senateTerms, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var congressionalTermWriter = await _congressionalTermInserterFactory.CreateAsync(connection);
        await using var senateTermWriter = await _senateTermInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var senateTerm in senateTerms) {
            await nodeWriter.InsertAsync(senateTerm);
            await searchableWriter.InsertAsync(senateTerm);
            await documentableWriter.InsertAsync(senateTerm);
            await congressionalTermWriter.InsertAsync(senateTerm);
            await senateTermWriter.InsertAsync(senateTerm);
            foreach (var partyAffiliation in senateTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = senateTerm.Id;
            }
            await _congressionalTermPoliticalPartyAffiliationCreator.CreateAsync(senateTerm.PartyAffiliations.ToAsyncEnumerable(), connection);

            foreach (var tenantNode in senateTerm.TenantNodes) {
                tenantNode.NodeId = senateTerm.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
