namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CongressionalTermPoliticalPartyAffiliationCreator : EntityCreator<CongressionalTermPoliticalPartyAffiliation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<Documentable> _documentableInserterFactory;
    private readonly IDatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation> _congressionalTermPoliticalPartyAffiliationInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public CongressionalTermPoliticalPartyAffiliationCreator(IDatabaseInserterFactory<Node> nodeInserterFactory, IDatabaseInserterFactory<Searchable> searchableInserterFactory, IDatabaseInserterFactory<Documentable> documentableInserterFactory, IDatabaseInserterFactory<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliationInserterFactory, IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _documentableInserterFactory = documentableInserterFactory;
        _congressionalTermPoliticalPartyAffiliationInserterFactory = congressionalTermPoliticalPartyAffiliationInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<CongressionalTermPoliticalPartyAffiliation> congressionalTermPoliticalPartyAffiliations, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var documentableWriter = await _documentableInserterFactory.CreateAsync(connection);
        await using var congressionalTermPoliticalPartyAffiliationWriter = await _congressionalTermPoliticalPartyAffiliationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var congressionalTermPoliticalPartyAffiliation in congressionalTermPoliticalPartyAffiliations) {
            await nodeWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            await searchableWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            await documentableWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            await congressionalTermPoliticalPartyAffiliationWriter.InsertAsync(congressionalTermPoliticalPartyAffiliation);
            foreach (var tenantNode in congressionalTermPoliticalPartyAffiliation.TenantNodes) {
                tenantNode.NodeId = congressionalTermPoliticalPartyAffiliation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
