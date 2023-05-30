using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToUpdateMapper : IEnumerableMapper<TenantNode.ExistingTenantNode, CreateModel.TenantNode.TenantNodeToCreateForExistingNode>
{
    public IEnumerable<CreateModel.TenantNode.TenantNodeToCreateForExistingNode> Map(IEnumerable<TenantNode.ExistingTenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (tenantNode.HasBeenDeleted)
                continue;
            yield return new CreateModel.TenantNode.TenantNodeToCreateForExistingNode {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = tenantNode.Id,
                },
                TenantId = tenantNode.Id,
                UrlId = tenantNode.UrlId,
                NodeId = tenantNode.Id,
                PublicationStatusId = tenantNode.PublicationStatusId,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
