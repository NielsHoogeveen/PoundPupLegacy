namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class HouseTermCreator : EntityCreator<HouseTerm>
{

    private readonly IDatabaseInserterFactory<HouseTerm> _houseTermInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<CongressionalTerm> _congressionalTermInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    private readonly IEntityCreator<CongressionalTermPoliticalPartyAffiliation> _CongressionalTermPoliticalPartyAffiliationCreator;
    public HouseTermCreator(
        IDatabaseInserterFactory<HouseTerm> houseTermInserterFactory,
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<Searchable> searchableInserterFactory,
        IDatabaseInserterFactory<Documentable> documentableInserterFactory,
        IDatabaseInserterFactory<CongressionalTerm> congressionalTermInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory,
        IEntityCreator<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationCreator
    )
    {
        _houseTermInserterFactory = houseTermInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _congressionalTermInserterFactory = congressionalTermInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _CongressionalTermPoliticalPartyAffiliationCreator = congressionalTermPoliticalPartyAffiliationCreator;
    }


    public override async Task CreateAsync(IAsyncEnumerable<HouseTerm> houseTerms, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var congressionalTermWriter = await _congressionalTermInserterFactory.CreateAsync(connection);
        await using var houseTermWriter = await _houseTermInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var houseTerm in houseTerms) {
            await nodeWriter.InsertAsync(houseTerm);
            await searchableWriter.InsertAsync(houseTerm);
            await documentableWriter.InsertAsync(houseTerm);
            await congressionalTermWriter.InsertAsync(houseTerm);
            await houseTermWriter.InsertAsync(houseTerm);
            foreach (var partyAffiliation in houseTerm.PartyAffiliations) {
                partyAffiliation.CongressionalTermId = houseTerm.Id;
            }
            await _CongressionalTermPoliticalPartyAffiliationCreator.CreateAsync(houseTerm.PartyAffiliations.ToAsyncEnumerable(), connection);
            foreach (var tenantNode in houseTerm.TenantNodes) {
                tenantNode.NodeId = houseTerm.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
