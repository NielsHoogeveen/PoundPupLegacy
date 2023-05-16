namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class NodeEditServiceBase<TEntity, TExisting, TNew, TCreate>
    where TEntity: class, Node
    where TExisting : ExistingNode, TEntity
    where TNew : NewNode, TEntity
    where TCreate : CreateModel.Node
{
    protected readonly NpgsqlConnection _connection;
    protected readonly ISaveService<IEnumerable<Tag>> _tagSaveService;
    protected readonly ISaveService<IEnumerable<TenantNode>> _tenantNodesSaveService;
    protected readonly ISaveService<IEnumerable<File>> _filesSaveService;
    private readonly ITenantRefreshService _tenantRefreshService;

    public NodeEditServiceBase(
        IDbConnection connection,

        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService

    )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _tagSaveService = tagSaveService;
        _tenantNodesSaveService = tenantNodesSaveService;
        _filesSaveService = filesSaveService;
        _tenantRefreshService = tenantRefreshService;
    }
    protected abstract Task StoreExisting(TExisting node, NpgsqlConnection connection);
    protected abstract Task<int> StoreNew(TNew node, NpgsqlConnection connection);
    protected virtual async Task StoreAdditional(TEntity node, int nodeId)
    {
        await _tagSaveService.SaveAsync(node.Tags.SelectMany(x => x.Entries), _connection);
        await _tenantNodesSaveService.SaveAsync(node.TenantNodes, _connection);
        await _filesSaveService.SaveAsync(node.Files, _connection);
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
        try {
            await _connection.OpenAsync();
            await using var tx = await _connection.BeginTransactionAsync();
            try {
                var id = await StoreNew(node, _connection);
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
                await StoreAdditional(node, id);
                await tx.CommitAsync();
                if (node.TenantNodes.Any(x => x.UrlPath is not null)) {
                    await _tenantRefreshService.Refresh();
                }
                return id;
            }
            catch (Exception) {
                await tx.RollbackAsync();
                throw;
            }
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }

    }
    protected virtual async Task<int> SaveAsync(TExisting node)
    {
        try {
            await _connection.OpenAsync();
            await using var tx = await _connection.BeginTransactionAsync();
            try {
                await StoreExisting(node, _connection);
                await StoreAdditional(node, node.NodeId);
                await tx.CommitAsync();
                if (node.TenantNodes.Any(x => x.UrlPath is not null)) {
                    await _tenantRefreshService.Refresh();
                }
                return node.UrlId;
            }
            catch (Exception) {
                await tx.RollbackAsync();
                throw;
            }
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
