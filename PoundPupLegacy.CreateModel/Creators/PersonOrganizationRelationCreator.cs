namespace PoundPupLegacy.CreateModel.Creators;

public class PersonOrganizationRelationCreator : IEntityCreator<PersonOrganizationRelation>
{
    public static async Task CreateAsync(IAsyncEnumerable<PersonOrganizationRelation> personOrganizationRelations, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var personOrganizationRelationWriter = await PersonOrganizationRelationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var personOrganizationRelation in personOrganizationRelations) {
            await nodeWriter.InsertAsync(personOrganizationRelation);
            await personOrganizationRelationWriter.InsertAsync(personOrganizationRelation);

            foreach (var tenantNode in personOrganizationRelation.TenantNodes) {
                tenantNode.NodeId = personOrganizationRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
