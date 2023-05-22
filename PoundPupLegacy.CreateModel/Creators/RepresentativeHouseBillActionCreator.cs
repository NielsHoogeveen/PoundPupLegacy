namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class RepresentativeHouseBillActionCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<NewRepresentativeHouseBillAction> representativeHouseBillActionInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewRepresentativeHouseBillAction>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewRepresentativeHouseBillAction> representativeHouseBillActions, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var representativeHouseBillActionWriter = await representativeHouseBillActionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
