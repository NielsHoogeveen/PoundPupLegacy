namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodesForCreateMapper : IEnumerableMapper<EditModel.TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.TenantNodeToCreateForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.TenantNodeToCreateForNewNode> Map(IEnumerable<EditModel.TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach (var item in source) {
            yield return new CreateModel.TenantNode.TenantNodeToCreateForNewNode {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = null,
                },
                PublicationStatusId = item.PublicationStatusId,
                SubgroupId = item.SubgroupId,
                TenantId = item.TenantId,
                UrlId = null,
                UrlPath = item.UrlPath,
            };
        }
    }
}
