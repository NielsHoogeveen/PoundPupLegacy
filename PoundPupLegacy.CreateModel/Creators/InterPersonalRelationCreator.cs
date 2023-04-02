namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreator : IEntityCreator<InterPersonalRelation>
{
    public async Task CreateAsync(IAsyncEnumerable<InterPersonalRelation> interPersonalRelations, IDbConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var interPersonalRelationWriter = await InterPersonalRelationInserter.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

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
