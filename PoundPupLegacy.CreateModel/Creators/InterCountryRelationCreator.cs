namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationCreator : EntityCreator<InterCountryRelation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<InterCountryRelation> _interCountryRelationInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public InterCountryRelationCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<InterCountryRelation> interCountryRelationInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _interCountryRelationInserterFactory = interCountryRelationInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<InterCountryRelation> interCountryRelations, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var interCountryRelationWriter = await _interCountryRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interCountryRelation in interCountryRelations) {
            await nodeWriter.InsertAsync(interCountryRelation);
            await interCountryRelationWriter.InsertAsync(interCountryRelation);

            foreach (var tenantNode in interCountryRelation.TenantNodes) {
                tenantNode.NodeId = interCountryRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
