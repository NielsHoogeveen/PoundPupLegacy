namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForNewNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.ToCreateForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreateForNewNode> Map(IEnumerable<TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach(var  tenantNode in source) {
            yield return new CreateModel.TenantNode.ToCreateForNewNode {
                IdentificationForCreate = new Identification.Possible {
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
