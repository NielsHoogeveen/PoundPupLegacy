using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForExistingNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, NewTenantNodeForExistingNode>
{
    public IEnumerable<NewTenantNodeForExistingNode> Map(IEnumerable<TenantNode.NewTenantNodeForExistingNode> source)
    {
        foreach(var  tenantNode in source) {
            yield return new NewTenantNodeForExistingNode {
                Id = null,
                PublicationStatusId = tenantNode.PublicationStatusId,
                TenantId = tenantNode.TenantId,
                UrlId = tenantNode.UrlId,
                NodeId = tenantNode.NodeId,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
