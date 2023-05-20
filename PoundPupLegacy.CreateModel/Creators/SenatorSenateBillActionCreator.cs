namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorSenateBillActionCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<SenatorSenateBillAction> senatorSenateBillActionInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<SenatorSenateBillAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<SenatorSenateBillAction> senatorSenateBillActions, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var senatorSenateBillActionWriter = await senatorSenateBillActionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
