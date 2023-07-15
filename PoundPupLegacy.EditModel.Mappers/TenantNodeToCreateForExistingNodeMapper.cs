namespace PoundPupLegacy.EditModel.Mappers;

internal class TenantNodeToCreateForExistingNodeMapper : IEnumerableMapper<TenantNode.ToCreateForExistingNode, DomainModel.TenantNode.ToCreate.ForExistingNode>
{
    public IEnumerable<DomainModel.TenantNode.ToCreate.ForExistingNode> Map(IEnumerable<TenantNode.ToCreateForExistingNode> source)
    {
        foreach (var tenantNode in source) {
            yield return new DomainModel.TenantNode.ToCreate.ForExistingNode {
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
