namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class SenatorSenateBillActionCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<NewSenatorSenateBillAction> senatorSenateBillActionInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewSenatorSenateBillAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewSenatorSenateBillAction> senatorSenateBillActions, IDbConnection connection)
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
