using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForNewNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, NewTenantNodeForNewNode>
{
    public IEnumerable<NewTenantNodeForNewNode> Map(IEnumerable<TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach(var  tenantNode in source) {
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
