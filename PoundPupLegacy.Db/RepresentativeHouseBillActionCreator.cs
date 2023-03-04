namespace PoundPupLegacy.Db;

public class RepresentativeHouseBillActionCreator : IEntityCreator<RepresentativeHouseBillAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<RepresentativeHouseBillAction> representativeHouseBillActions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var representativeHouseBillActionWriter = await RepresentativeHouseBillActionWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var representativeHouseBillAction in representativeHouseBillActions) {
            await nodeWriter.WriteAsync(representativeHouseBillAction);
            await representativeHouseBillActionWriter.WriteAsync(representativeHouseBillAction);

            foreach (var tenantNode in representativeHouseBillAction.TenantNodes) {
                tenantNode.NodeId = representativeHouseBillAction.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
