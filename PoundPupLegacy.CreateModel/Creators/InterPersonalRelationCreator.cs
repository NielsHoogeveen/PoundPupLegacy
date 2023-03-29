namespace PoundPupLegacy.CreateModel.Creators;

public class InterPersonalRelationCreator : IEntityCreator<InterPersonalRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterPersonalRelation> interPersonalRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var interPersonalRelationWriter = await InterPersonalRelationWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var interPersonalRelation in interPersonalRelations) {
            await nodeWriter.WriteAsync(interPersonalRelation);
            await interPersonalRelationWriter.WriteAsync(interPersonalRelation);

            foreach (var tenantNode in interPersonalRelation.TenantNodes) {
                tenantNode.NodeId = interPersonalRelation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
