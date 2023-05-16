namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TenantNodesSaveService : ISaveService<IEnumerable<TenantNode>>
{
    private readonly IDatabaseDeleterFactory<TenantNodeDeleterRequest> _tenantNodeDeleterFactory;
    private readonly IDatabaseUpdaterFactory<TenantNodeUpdaterRequest> _tenantNodeUpdaterFactory;
    private readonly IDatabaseInserterFactory<CreateModel.TenantNode> _tenantNodeInserterFactory;

    public TenantNodesSaveService(
        IDatabaseDeleterFactory<TenantNodeDeleterRequest> tenantNodeDeleterFactory,
        IDatabaseUpdaterFactory<TenantNodeUpdaterRequest> tenantNodeUpdaterFactory,
        IDatabaseInserterFactory<CreateModel.TenantNode> tenantNodeInserterFactory
    )
    {
        _tenantNodeDeleterFactory = tenantNodeDeleterFactory;
        _tenantNodeUpdaterFactory = tenantNodeUpdaterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public async Task SaveAsync(
        IEnumerable<TenantNode> tenantNodes,
        IDbConnection connection
        )
    {
        if (tenantNodes.Any(x => x.HasBeenDeleted)) {
            await using var deleter = await _tenantNodeDeleterFactory.CreateAsync(connection);
            foreach (var tenantNode in tenantNodes.Where(x => x.HasBeenDeleted)) {
                if (tenantNode is not null && tenantNode.Id.HasValue) {
                    await deleter.DeleteAsync(new TenantNodeDeleterRequest { Id = tenantNode.Id.Value });
                }
            }
        }
        if (tenantNodes.Any(x => x.Id is null)) {
            await using var inserter = await _tenantNodeInserterFactory.CreateAsync(connection);
            foreach (var tenantNode in tenantNodes.Where(x => !x.Id.HasValue)) {
                var tenantNodeToCreate = new CreateModel.TenantNode {
                    Id = tenantNode.Id,
                    TenantId = tenantNode.TenantId,
                    NodeId = tenantNode.NodeId,
                    UrlId = tenantNode.UrlId,
                    UrlPath = tenantNode.UrlPath,
                    SubgroupId = tenantNode.SubgroupId,
                    PublicationStatusId = tenantNode.PublicationStatusId
                };
                await inserter.InsertAsync(tenantNodeToCreate);
            }
        }
        if (tenantNodes.Any(x => x.Id.HasValue)) {
            await using var updater = await _tenantNodeUpdaterFactory.CreateAsync(connection);
            foreach (var tenantNode in tenantNodes.Where(x => x.Id.HasValue)) {
                var tenantNodeUpdate = new TenantNodeUpdaterRequest {
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
