namespace PoundPupLegacy.CreateModel.Creators;

public class SenatorSenateBillActionCreator : IEntityCreator<SenatorSenateBillAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<SenatorSenateBillAction> senatorSenateBillActions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var senatorSenateBillActionWriter = await SenatorSenateBillActionWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var senatorSenateBillAction in senatorSenateBillActions) {
            await nodeWriter.WriteAsync(senatorSenateBillAction);
            await senatorSenateBillActionWriter.WriteAsync(senatorSenateBillAction);

            foreach (var tenantNode in senatorSenateBillAction.TenantNodes) {
                tenantNode.NodeId = senatorSenateBillAction.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
