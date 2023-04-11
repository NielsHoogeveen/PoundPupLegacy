namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PartyPoliticalEntityRelationCreator : EntityCreator<PartyPoliticalEntityRelation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<PartyPoliticalEntityRelation> _partyPoliticalEntityRelationInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public PartyPoliticalEntityRelationCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<PartyPoliticalEntityRelation> partyPoliticalEntityRelationInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _partyPoliticalEntityRelationInserterFactory = partyPoliticalEntityRelationInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<PartyPoliticalEntityRelation> partyPoliticalEntityRelations, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var partyPoliticalEntityRelationWriter = await _partyPoliticalEntityRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var partyPoliticalEntityRelation in partyPoliticalEntityRelations) {
            await nodeWriter.InsertAsync(partyPoliticalEntityRelation);
            await partyPoliticalEntityRelationWriter.InsertAsync(partyPoliticalEntityRelation);

            foreach (var tenantNode in partyPoliticalEntityRelation.TenantNodes) {
                tenantNode.NodeId = partyPoliticalEntityRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
