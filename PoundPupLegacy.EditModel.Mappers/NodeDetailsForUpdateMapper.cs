using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class NodeDetailsForUpdateMapper(
    IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd> nodeTermsToAddMapper,
    IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove> nodeTermsToDeleteMapper,
    IEnumerableMapper<TenantNode.ToCreateForExistingNode, CreateModel.TenantNode.ToCreate.ForExistingNode> tenantNodeToCreateMapper,
    IEnumerableMapper<TenantNode.ToUpdate, CreateModel.Deleters.TenantNodeToDelete> tenantNodeDeleteMapper,
    IEnumerableMapper<TenantNode.ToUpdate, CreateModel.TenantNode.ToUpdate> tenantNodeUpdateMapper
    ) : IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate>
{
    public CreateModel.NodeDetails.ForUpdate Map(NodeDetails.ForUpdate source)
    {
        var now = DateTime.Now;
        return new CreateModel.NodeDetails.ForUpdate {
            AuthoringStatusId = 1,
            ChangedDateTime = now,
            Title = source.Title,
            TenantNodesToAdd = tenantNodeToCreateMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToAdd).ToList(),
            TenantNodesToRemove = tenantNodeDeleteMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToUpdate).ToList(),
            TenantNodesToUpdate = tenantNodeUpdateMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToUpdate).ToList(),
            NodeTermsToAdd = nodeTermsToAddMapper.Map(source.TagsForUpdate).ToList(),
            NodeTermsToRemove = nodeTermsToDeleteMapper.Map(source.TagsForUpdate).ToList(),

        };
    }
}
