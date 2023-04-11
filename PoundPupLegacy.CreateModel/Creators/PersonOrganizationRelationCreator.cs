namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PersonOrganizationRelationCreator : EntityCreator<PersonOrganizationRelation>
{
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<PersonOrganizationRelation> _personOrganizationRelationInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public PersonOrganizationRelationCreator(
        IDatabaseInserterFactory<Node> nodeInserterFactory,
        IDatabaseInserterFactory<PersonOrganizationRelation> personOrganizationRelationInserterFactory,
        IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _nodeInserterFactory = nodeInserterFactory;
        _personOrganizationRelationInserterFactory = personOrganizationRelationInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }

    public override async Task CreateAsync(IAsyncEnumerable<PersonOrganizationRelation> personOrganizationRelations, IDbConnection connection)
    {
        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var personOrganizationRelationWriter = await _personOrganizationRelationInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);

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
