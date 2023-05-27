using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForExistingNodeMapper : IEnumerableMapper<TenantNode, NewTenantNodeForExistingNode>
{
    public IEnumerable<NewTenantNodeForExistingNode> Map(IEnumerable<TenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (tenantNode.Id.HasValue)
                continue;
            if(!tenantNode.NodeId.HasValue)
                continue;
            yield return new NewTenantNodeForExistingNode {
                Id = null,
                PublicationStatusId = tenantNode.PublicationStatusId,
                TenantId = tenantNode.TenantId,
                UrlId = tenantNode.NodeId.Value,
                NodeId = tenantNode.NodeId.Value,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
