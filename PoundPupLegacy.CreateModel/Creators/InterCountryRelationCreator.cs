namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterCountryRelationCreator(
    IDatabaseInserterFactory<Node> nodeInserterFactory,
    IDatabaseInserterFactory<InterCountryRelation> interCountryRelationInserterFactory,
    IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
) : EntityCreator<InterCountryRelation>
{
    public override async Task CreateAsync(IAsyncEnumerable<InterCountryRelation> interCountryRelations, IDbConnection connection)
    {
        await using var nodeWriter = await nodeInserterFactory.CreateAsync(connection);
        await using var interCountryRelationWriter = await interCountryRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await tenantNodeInserterFactory.CreateAsync(connection);

        await foreach (var interCountryRelation in interCountryRelations) {
            await nodeWriter.InsertAsync(interCountryRelation);
            await interCountryRelationWriter.InsertAsync(interCountryRelation);

            foreach (var tenantNode in interCountryRelation.TenantNodes) {
                tenantNode.NodeId = interCountryRelation.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }
        }
    }
}
