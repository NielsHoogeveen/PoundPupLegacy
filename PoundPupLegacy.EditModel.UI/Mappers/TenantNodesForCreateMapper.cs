namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodesForCreateMapper : IEnumerableMapper<EditModel.TenantNode.NewTenantNodeForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreate.ForNewNode> Map(IEnumerable<EditModel.TenantNode.NewTenantNodeForNewNode> source)
    {
        foreach (var item in source) {
            yield return new CreateModel.TenantNode.ToCreate.ForNewNode {
                Identification = new Identification.Possible {
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
