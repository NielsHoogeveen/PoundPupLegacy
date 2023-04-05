namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class InterOrganizationalRelationCreator : EntityCreator<InterOrganizationalRelation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<InterOrganizationalRelation> _interOrganizationalRelationInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public InterOrganizationalRelationCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<InterOrganizationalRelation> interOrganizationalRelationInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory
    )
    {
        _interOrganizationalRelationInserterFactory = interOrganizationalRelationInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<InterOrganizationalRelation> interOrganizationalRelations, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var interOrganizationalRelationWriter = await _interOrganizationalRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
