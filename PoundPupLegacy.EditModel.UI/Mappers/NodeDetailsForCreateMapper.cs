namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeDetailsForCreateMapper(
    IEnumerableMapper<Tags, int> termIdsToAddMapper,
    IEnumerableMapper<EditModel.TenantNode.ToCreateForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode> tenantNodeMapper
    ) : IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate>
{
    public CreateModel.NodeDetails.ForCreate Map(EditModel.NodeDetails.ForCreate source)
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
            TermIds = termIdsToAddMapper.Map(source.TagsToCreate).ToList(),
            TenantNodes = tenantNodeMapper.Map(source.TenantNodeDetailsForCreate.TenantNodesToAdd).ToList(),
        };
    }
}
