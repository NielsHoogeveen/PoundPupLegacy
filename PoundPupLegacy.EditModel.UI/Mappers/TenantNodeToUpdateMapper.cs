using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToUpdateMapper : IEnumerableMapper<TenantNode.ExistingTenantNode, CreateModel.TenantNode.ToCreateForExistingNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreateForExistingNode> Map(IEnumerable<TenantNode.ExistingTenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (tenantNode.HasBeenDeleted)
                continue;
            yield return new CreateModel.TenantNode.ToCreateForExistingNode {
                IdentificationForCreate = new Identification.Possible {
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
