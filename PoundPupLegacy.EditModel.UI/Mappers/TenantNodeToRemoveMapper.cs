using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToRemoveMapper : IEnumerableMapper<TenantNode, TenantNodeToDelete>
{
    public IEnumerable<TenantNodeToDelete> Map(IEnumerable<TenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (!tenantNode.Id.HasValue)
                continue;
            if (!tenantNode.HasBeenDeleted)
                continue;
            yield return new TenantNodeToDelete { Id = tenantNode.Id.Value };
        }
    }
}
