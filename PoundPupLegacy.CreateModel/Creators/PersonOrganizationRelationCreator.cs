namespace PoundPupLegacy.CreateModel.Creators;

public class PersonOrganizationRelationCreator : IEntityCreator<PersonOrganizationRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<PersonOrganizationRelation> personOrganizationRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var personOrganizationRelationWriter = await PersonOrganizationRelationWriter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var personOrganizationRelation in personOrganizationRelations) {
            await nodeWriter.WriteAsync(personOrganizationRelation);
            await personOrganizationRelationWriter.WriteAsync(personOrganizationRelation);

            foreach (var tenantNode in personOrganizationRelation.TenantNodes) {
                tenantNode.NodeId = personOrganizationRelation.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
