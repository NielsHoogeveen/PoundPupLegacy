namespace PoundPupLegacy.Db;

public class InterOrganizationalRelationCreator : IEntityCreator<InterOrganizationalRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterOrganizationalRelation> interOrganizationalRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var interOrganizationalRelationWriter = await InterOrganizationalRelationWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var interOrganizationalRelation in interOrganizationalRelations)
        {
            await nodeWriter.WriteAsync(interOrganizationalRelation);
            await interOrganizationalRelationWriter.WriteAsync(interOrganizationalRelation);

            foreach (var tenantNode in interOrganizationalRelation.TenantNodes)
            {
                tenantNode.NodeId = interOrganizationalRelation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
