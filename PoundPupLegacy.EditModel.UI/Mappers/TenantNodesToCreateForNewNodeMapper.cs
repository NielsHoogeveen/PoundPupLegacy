namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodesToCreateForNewNodeMapper : IEnumerableMapper<EditModel.TenantNode.ToCreateForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode>
{
    public IEnumerable<CreateModel.TenantNode.ToCreate.ForNewNode> Map(IEnumerable<EditModel.TenantNode.ToCreateForNewNode> source)
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
