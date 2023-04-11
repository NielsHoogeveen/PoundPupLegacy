namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorSenateBillActionCreator : EntityCreator<SenatorSenateBillAction>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<SenatorSenateBillAction> _senatorSenateBillActionInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public SenatorSenateBillActionCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<SenatorSenateBillAction> senatorSenateBillActionInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _nodeInserterFactory = nodeInserterFactory;
        _senatorSenateBillActionInserterFactory = senatorSenateBillActionInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<SenatorSenateBillAction> senatorSenateBillActions, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var senatorSenateBillActionWriter = await _senatorSenateBillActionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var senatorSenateBillAction in senatorSenateBillActions) {
            await nodeWriter.InsertAsync(senatorSenateBillAction);
            await senatorSenateBillActionWriter.InsertAsync(senatorSenateBillAction);

            foreach (var tenantNode in senatorSenateBillAction.TenantNodes) {
                tenantNode.NodeId = senatorSenateBillAction.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
