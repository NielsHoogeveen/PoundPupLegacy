namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreator : IEntityCreator<InterOrganizationalRelation>
{
    public async Task CreateAsync(IAsyncEnumerable<InterOrganizationalRelation> interOrganizationalRelations, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var interOrganizationalRelationWriter = await InterOrganizationalRelationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var interOrganizationalRelation in interOrganizationalRelations) {
            await nodeWriter.InsertAsync(interOrganizationalRelation);
            await interOrganizationalRelationWriter.InsertAsync(interOrganizationalRelation);

            foreach (var tenantNode in interOrganizationalRelation.TenantNodes) {
                tenantNode.NodeId = interOrganizationalRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
