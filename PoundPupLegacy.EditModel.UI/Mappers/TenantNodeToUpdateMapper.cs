namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToUpdateMapper : IEnumerableMapper<TenantNode.ToUpdate, CreateModel.TenantNode.ToUpdate>
{
    public IEnumerable<CreateModel.TenantNode.ToUpdate> Map(IEnumerable<TenantNode.ToUpdate> source)
    {
        foreach(var  tenantNode in source) {
            if (tenantNode.HasBeenDeleted)
                continue;
            yield return new CreateModel.TenantNode.ToUpdate {
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
