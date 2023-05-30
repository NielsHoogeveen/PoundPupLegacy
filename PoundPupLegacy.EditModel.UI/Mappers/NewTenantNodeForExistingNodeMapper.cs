using PoundPupLegacy.CreateModel;
namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForExistingNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, CreateModel.TenantNode.ToCreateForExistingNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreateForExistingNode> Map(IEnumerable<TenantNode.NewTenantNodeForExistingNode> source)
    {
        foreach(var  tenantNode in source) {
            yield return new CreateModel.TenantNode.ToCreateForExistingNode {
                Identification = new Identification.Possible {
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
