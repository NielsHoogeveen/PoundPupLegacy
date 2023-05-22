namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<NewInterOrganizationalRelation> interOrganizationalRelationInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewInterOrganizationalRelation>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewInterOrganizationalRelation> interOrganizationalRelations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var interOrganizationalRelationWriter = await interOrganizationalRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
