using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToUpdateMapper : IEnumerableMapper<TenantNode.ExistingTenantNode, ExistingTenantNode>
{
    public IEnumerable<ExistingTenantNode> Map(IEnumerable<TenantNode.ExistingTenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (tenantNode.HasBeenDeleted)
                continue;
            yield return new ExistingTenantNode {
                Id = tenantNode.Id,
                PublicationStatusId = tenantNode.PublicationStatusId,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
