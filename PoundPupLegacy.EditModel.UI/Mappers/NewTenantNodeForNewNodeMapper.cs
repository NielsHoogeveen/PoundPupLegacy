using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForNewNodeMapper : IEnumerableMapper<TenantNode, NewTenantNodeForNewNode>
{
    public IEnumerable<NewTenantNodeForNewNode> Map(IEnumerable<TenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (tenantNode.Id.HasValue)
                continue;
            if(tenantNode.NodeId.HasValue)
                continue;
            yield return new NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tenantNode.PublicationStatusId,
                TenantId = tenantNode.TenantId,
                UrlId = null,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
