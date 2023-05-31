using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeDetailsForUpdateMapper(
    IEnumerableMapper<Tags, NodeTermToAdd> nodeTermsToAddMapper,
    IEnumerableMapper<Tags, NodeTermToRemove> nodeTermsToDeleteMapper,
    IEnumerableMapper<EditModel.TenantNode.ToCreateForExistingNode, CreateModel.TenantNode.ToCreate.ForExistingNode> tenantNodeToCreateMapper,
    IEnumerableMapper<EditModel.TenantNode.ToUpdate, CreateModel.Deleters.TenantNodeToDelete> tenantNodeDeleteMapper,
    IEnumerableMapper<EditModel.TenantNode.ToUpdate, CreateModel.TenantNode.ToUpdate> tenantNodeUpdateMapper
    ) : IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate>
{
    public CreateModel.NodeDetails.ForUpdate Map(EditModel.NodeDetails.ForUpdate source)
    {
        var now = DateTime.Now;
        return new CreateModel.NodeDetails.ForUpdate {
            AuthoringStatusId = 1,
            ChangedDateTime = now,
            Title = source.Title,
            TenantNodesToAdd = tenantNodeToCreateMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToAdd).ToList(),
            TenantNodesToRemove = tenantNodeDeleteMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToUpdate).ToList(),
            TenantNodesToUpdate = tenantNodeUpdateMapper.Map(source.TenantNodeDetailsForUpdate.TenantNodesToUpdate).ToList(),
            NodeTermsToAdd = nodeTermsToAddMapper.Map(source.Tags).ToList(),
            NodeTermsToRemove = nodeTermsToDeleteMapper.Map(source.Tags).ToList(),

        };
    }
}
