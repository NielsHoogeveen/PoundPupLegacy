namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<NewPersonOrganizationRelation> personOrganizationRelationInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<NewPersonOrganizationRelation>
{
    public override async Task CreateAsync(IAsyncEnumerable<NewPersonOrganizationRelation> personOrganizationRelations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var personOrganizationRelationWriter = await personOrganizationRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

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
