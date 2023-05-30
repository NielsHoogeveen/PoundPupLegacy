namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewTenantNodeForNewNodeMapper : IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreate.ForNewNode> Map(IEnumerable<TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach(var  tenantNode in source) {
            yield return new CreateModel.TenantNode.ToCreate.ForNewNode {
                Identification = new Identification.Possible {
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
