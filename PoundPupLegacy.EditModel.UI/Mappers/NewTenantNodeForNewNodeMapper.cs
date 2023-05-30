namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForNewNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.TenantNodeToCreateForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.TenantNodeToCreateForNewNode> Map(IEnumerable<TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach(var  tenantNode in source) {
            yield return new CreateModel.TenantNode.TenantNodeToCreateForNewNode {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = null,
                },
                PublicationStatusId = tenantNode.PublicationStatusId,
                TenantId = tenantNode.TenantId,
                UrlId = null,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
