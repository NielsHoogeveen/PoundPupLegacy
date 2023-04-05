namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterPersonalRelationCreator : EntityCreator<InterPersonalRelation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<InterPersonalRelation> _interPersonalRelationInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public InterPersonalRelationCreator(IDatabaseInserterFactory<Node> nodeInserterFactory, IDatabaseInserterFactory<InterPersonalRelation> interPersonalRelationInserterFactory, IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _nodeInserterFactory = nodeInserterFactory;
        _interPersonalRelationInserterFactory = interPersonalRelationInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<InterPersonalRelation> interPersonalRelations, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var interPersonalRelationWriter = await _interPersonalRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
