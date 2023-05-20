using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class NodeEditServiceBase<TEntity, TExisting, TNew, TCreate>(
        IDbConnection connection,
        ILogger logger,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService

    ): DatabaseService(connection, logger)
    where TEntity : class, Node
    where TExisting : ExistingNode, TEntity
    where TNew : NewNode, TEntity
    where TCreate : CreateModel.Node
{
    protected abstract Task StoreExisting(TExisting node, NpgsqlConnection connection);
    protected abstract Task<int> StoreNew(TNew node, NpgsqlConnection connection);
    protected virtual async Task StoreAdditional(TEntity node, int nodeId, NpgsqlConnection connection)
    {
        await tagSaveService.SaveAsync(node.Tags.SelectMany(x => x.Entries), connection);
        await tenantNodesSaveService.SaveAsync(node.TenantNodes, connection);
        await filesSaveService.SaveAsync(node.Files, connection);
    }

    public async Task<int> SaveAsync(TEntity node)
    {
        return await (node switch {
            TNew n => SaveAsync(n),
            TExisting e => SaveAsync(e),
            _ => throw new Exception("Cannot reach")
        });
    }

    protected virtual async Task<int> SaveAsync(TNew node)
    {
        return await WithTransactedConnection(async (connection) => { 
            var id = await StoreNew(node, connection);
            foreach (var tagNodeType in node.Tags) {
                foreach (var tag in tagNodeType.Entries) {
                    tag.NodeId = id;
                }
            }
            foreach (var tenantNode in node.TenantNodes) {
                tenantNode.NodeId = id;
            }
            foreach (var file in node.Files) {
                file.NodeId = id;
            }
            await StoreAdditional(node, id, connection);
            if (node.TenantNodes.Any(x => x.UrlPath is not null)) {
                await tenantRefreshService.Refresh();
            }
            return id;
        });
    }
    protected virtual async Task<int> SaveAsync(TExisting node)
    {
        return await WithTransactedConnection(async (connection) => {
            await StoreExisting(node, connection);
            await StoreAdditional(node, node.NodeId, connection);
            if (node.TenantNodes.Any(x => x.UrlPath is not null)) {
                await tenantRefreshService.Refresh();
            }
            return node.UrlId;
        });
    }
}
