namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<InterPersonalRelation> interPersonalRelationInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<InterPersonalRelation>
{
    public override async Task CreateAsync(IAsyncEnumerable<InterPersonalRelation> interPersonalRelations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var interPersonalRelationWriter = await interPersonalRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interPersonalRelation in interPersonalRelations) {
            await nodeWriter.InsertAsync(interPersonalRelation);
            await interPersonalRelationWriter.InsertAsync(interPersonalRelation);

            foreach (var tenantNode in interPersonalRelation.TenantNodes) {
                tenantNode.NodeId = interPersonalRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
