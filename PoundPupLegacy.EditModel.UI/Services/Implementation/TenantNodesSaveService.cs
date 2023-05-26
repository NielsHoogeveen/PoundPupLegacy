using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TenantNodesSaveService(
    IDatabaseDeleterFactory<TenantNodeDeleterRequest> tenantNodeDeleterFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeUpdaterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableTenantNodeForExistingNode> tenantNodeInserterFactory
) : ISaveService<IEnumerable<TenantNode>>
{
    public async Task SaveAsync(
        IEnumerable<TenantNode> tenantNodes,
        IDbConnection connection
        )
    {
        if (tenantNodes.Any(x => x.HasBeenDeleted)) {
            await using var deleter = await tenantNodeDeleterFactory.CreateAsync(connection);
            foreach (var tenantNode in tenantNodes.Where(x => x.HasBeenDeleted)) {
                if (tenantNode is not null && tenantNode.Id.HasValue) {
                    await deleter.DeleteAsync(new TenantNodeDeleterRequest { Id = tenantNode.Id.Value });
                }
            }
        }
        if (tenantNodes.Any(x => x.Id is null)) {
            await using var inserter = await tenantNodeInserterFactory.CreateAsync(connection);
            foreach (var tenantNode in tenantNodes.Where(x => !x.Id.HasValue)) {
                var tenantNodeToCreate = new NewTenantNodeForExistingNode {
                    Id = tenantNode.Id,
                    TenantId = tenantNode.TenantId,
                    NodeId = tenantNode.NodeId!.Value,
                    UrlId = tenantNode.UrlId!.Value,
                    UrlPath = tenantNode.UrlPath,
                    SubgroupId = tenantNode.SubgroupId,
                    PublicationStatusId = tenantNode.PublicationStatusId
                };
                await inserter.InsertAsync(tenantNodeToCreate);
            }
        }
        if (tenantNodes.Any(x => x.Id.HasValue)) {
            await using var updater = await tenantNodeUpdaterFactory.CreateAsync(connection);
            foreach (var tenantNode in tenantNodes.Where(x => x.Id.HasValue)) {
                var tenantNodeUpdate = new CreateModel.ExistingTenantNode {
                    Id = tenantNode.Id!.Value,
                    UrlPath = tenantNode.UrlPath,
                    SubgroupId = tenantNode.SubgroupId,
                    PublicationStatusId = tenantNode.PublicationStatusId
                };
                await updater.UpdateAsync(tenantNodeUpdate);
            }
        }
    }
}
