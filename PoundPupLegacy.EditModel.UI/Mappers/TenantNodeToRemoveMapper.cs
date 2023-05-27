using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TenantNodeToRemoveMapper : IEnumerableMapper<TenantNode.ExistingTenantNode, TenantNodeToDelete>
{
    public IEnumerable<TenantNodeToDelete> Map(IEnumerable<TenantNode.ExistingTenantNode> source)
    {
        foreach(var  tenantNode in source) {
            if (!tenantNode.HasBeenDeleted)
                continue;
            yield return new TenantNodeToDelete { Id = tenantNode.Id};
        }
    }
}
