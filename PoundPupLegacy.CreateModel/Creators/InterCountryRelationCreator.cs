namespace PoundPupLegacy.CreateModel.Creators;

public class InterCountryRelationCreator : IEntityCreator<InterCountryRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterCountryRelation> interCountryRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var interCountryRelationWriter = await InterCountryRelationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
