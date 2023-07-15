namespace PoundPupLegacy.EditModel.Mappers;

internal class TenantNodeToUpdateMapper : IEnumerableMapper<TenantNode.ToUpdate, DomainModel.TenantNode.ToUpdate>
{
    public IEnumerable<DomainModel.TenantNode.ToUpdate> Map(IEnumerable<TenantNode.ToUpdate> source)
    {
        foreach (var tenantNode in source) {
            if (tenantNode.HasBeenDeleted)
                continue;
            yield return new DomainModel.TenantNode.ToUpdate {
                Identification = new Identification.Certain {
                    Id = tenantNode.Id,
                },
                TenantId = tenantNode.Id,
                PublicationStatusId = tenantNode.PublicationStatusId,
                UrlPath = tenantNode.UrlPath,
                SubgroupId = tenantNode.SubgroupId,
            };
        }
    }
}
