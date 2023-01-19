namespace PoundPupLegacy.Db;

public class InterCountryRelationCreator : IEntityCreator<InterCountryRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterCountryRelation> interCountryRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var interCountryRelationWriter = await InterCountryRelationWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var interCountryRelation in interCountryRelations)
        {
            await nodeWriter.WriteAsync(interCountryRelation);
            await documentableWriter.WriteAsync(interCountryRelation);
            await interCountryRelationWriter.WriteAsync(interCountryRelation);

            foreach (var tenantNode in interCountryRelation.TenantNodes)
            {
                tenantNode.NodeId = interCountryRelation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
