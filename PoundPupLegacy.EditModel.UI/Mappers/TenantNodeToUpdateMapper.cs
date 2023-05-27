using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToUpdateMapper : IEnumerableMapper<TenantNode, ExistingTenantNode>
{
    public IEnumerable<ExistingTenantNode> Map(IEnumerable<TenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (!tenantNode.Id.HasValue)
                continue;
            if (tenantNode.HasBeenDeleted)
                continue;
            yield return new ExistingTenantNode {
                Id = tenantNode.Id.Value,
                PublicationStatusId = tenantNode.PublicationStatusId,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
