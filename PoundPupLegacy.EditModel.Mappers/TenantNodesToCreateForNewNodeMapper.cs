namespace PoundPupLegacy.EditModel.Mappers;

internal class TenantNodesToCreateForNewNodeMapper : IEnumerableMapper<TenantNode.ToCreateForNewNode, DomainModel.TenantNode.ToCreate.ForNewNode>
{
    public IEnumerable<DomainModel.TenantNode.ToCreate.ForNewNode> Map(IEnumerable<TenantNode.ToCreateForNewNode> source)
    {
        foreach (var item in source) {
            yield return new DomainModel.TenantNode.ToCreate.ForNewNode {
                Identification = new Identification.Possible {
                    Id = null,
                },
                PublicationStatusId = item.PublicationStatusId,
                SubgroupId = item.SubgroupId,
                TenantId = item.TenantId,
                UrlId = null,
            };
        }
    }
}
