namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class RepresentativeHouseBillActionCreator : EntityCreator<RepresentativeHouseBillAction>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<RepresentativeHouseBillAction> _representativeHouseBillActionInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public RepresentativeHouseBillActionCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<RepresentativeHouseBillAction> representativeHouseBillActionInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _representativeHouseBillActionInserterFactory = representativeHouseBillActionInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<RepresentativeHouseBillAction> representativeHouseBillActions, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var representativeHouseBillActionWriter = await _representativeHouseBillActionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var representativeHouseBillAction in representativeHouseBillActions) {
            await nodeWriter.InsertAsync(representativeHouseBillAction);
            await representativeHouseBillActionWriter.InsertAsync(representativeHouseBillAction);

            foreach (var tenantNode in representativeHouseBillAction.TenantNodes) {
                tenantNode.NodeId = representativeHouseBillAction.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
