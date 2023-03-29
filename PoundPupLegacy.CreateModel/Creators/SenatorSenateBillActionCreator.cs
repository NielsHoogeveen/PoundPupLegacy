namespace PoundPupLegacy.CreateModel.Creators;

public class SenatorSenateBillActionCreator : IEntityCreator<SenatorSenateBillAction>
{
    public static async Task CreateAsync(IAsyncEnumerable<SenatorSenateBillAction> senatorSenateBillActions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var senatorSenateBillActionWriter = await SenatorSenateBillActionInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
