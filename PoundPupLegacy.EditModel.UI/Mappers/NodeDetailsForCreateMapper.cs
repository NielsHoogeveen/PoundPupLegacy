namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeDetailsForCreateMapper(
    IEnumerableMapper<Tags, int> termIdsToAddMapper,
    IEnumerableMapper<EditModel.TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.ToCreateForNewNode> tenantNodeMapper
    ) : IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.ForCreate>
{
    public CreateModel.NodeDetails.ForCreate Map(EditModel.NodeDetails.NodeDetailsForCreate source)
    {
        var now = DateTime.Now;
        return new CreateModel.NodeDetails.ForCreate {
            AuthoringStatusId = 1,
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = source.NodeTypeId,
            OwnerId = source.OwnerId,
            PublisherId = source.PublisherId,
            Title = source.Title,
            TermIds = termIdsToAddMapper.Map(source.Tags).ToList(),
            TenantNodes = tenantNodeMapper.Map(source.NewTenantNodeDetails.TenantNodesToAdd).ToList(),
        };
    }
}
