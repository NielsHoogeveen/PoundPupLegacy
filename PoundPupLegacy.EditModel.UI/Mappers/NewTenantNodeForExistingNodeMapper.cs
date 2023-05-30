using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForExistingNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, CreateModel.TenantNode.TenantNodeToCreateForExistingNode>
{
    public IEnumerable<CreateModel.TenantNode.TenantNodeToCreateForExistingNode> Map(IEnumerable<TenantNode.NewTenantNodeForExistingNode> source)
    {
        foreach(var  tenantNode in source) {
            yield return new CreateModel.TenantNode.TenantNodeToCreateForExistingNode {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = null,
                },
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
