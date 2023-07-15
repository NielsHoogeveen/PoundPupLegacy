using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.Mappers;

internal class TenantNodeToRemoveMapper : IEnumerableMapper<TenantNode.ToUpdate, TenantNodeToDelete>
{
    public IEnumerable<TenantNodeToDelete> Map(IEnumerable<TenantNode.ToUpdate> source)
    {
        foreach (var tenantNode in source) {
            if (!tenantNode.HasBeenDeleted)
                continue;
            yield return new TenantNodeToDelete { Id = tenantNode.Id };
        }
    }
}
