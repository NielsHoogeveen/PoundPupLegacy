namespace PoundPupLegacy.CreateModel.Creators;

public class RepresentativeHouseBillActionCreator : IEntityCreator<RepresentativeHouseBillAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<RepresentativeHouseBillAction> representativeHouseBillActions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var representativeHouseBillActionWriter = await RepresentativeHouseBillActionInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
