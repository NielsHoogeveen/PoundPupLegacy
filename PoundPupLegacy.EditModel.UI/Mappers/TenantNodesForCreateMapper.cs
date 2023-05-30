namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodesForCreateMapper : IEnumerableMapper<EditModel.TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.ToCreateForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreateForNewNode> Map(IEnumerable<EditModel.TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach (var item in source) {
            yield return new CreateModel.TenantNode.ToCreateForNewNode {
                IdentificationForCreate = new Identification.Possible {
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
