using Npgsql;
using PoundPupLegacy.EditModel;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

public abstract class NodeEditServiceBase<T, TCreate>
    where T : Node
    where TCreate : CreateModel.Node
{
    protected readonly NpgsqlConnection _connection;
    protected readonly ISiteDataService _siteDateService;
    protected readonly INodeCacheService _nodeCacheService;
    protected readonly ISaveService<IEnumerable<Tag>> _tagSaveService;
    protected readonly ISaveService<IEnumerable<TenantNode>> _tenantNodesSaveService;
    protected readonly ISaveService<IEnumerable<File>> _filesSaveService;
    protected readonly ILogger _logger;

    public NodeEditServiceBase(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ILogger logger
    )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _siteDateService = siteDataService;
        _nodeCacheService = nodeCacheService;
        _tagSaveService = tagSaveService;
        _tenantNodesSaveService = tenantNodesSaveService;
        _filesSaveService = filesSaveService;
        _logger = logger;

    }
    protected abstract Task StoreExisting(T node, NpgsqlConnection connection);
    protected abstract Task StoreNew(T node, NpgsqlConnection connection);
    protected virtual async Task StoreAdditional(T node)
    {
        await _tagSaveService.SaveAsync(node.Tags, _connection);
        await _tenantNodesSaveService.SaveAsync(node.TenantNodes, _connection);
        await _filesSaveService.SaveAsync(node.Files, _connection);
    }

    public virtual async Task SaveAsync(T node)
    {
        try {
            await _connection.OpenAsync();
            await using var tx = await _connection.BeginTransactionAsync();
            try {
                if (node.NodeId is null) {
                    await StoreNew(node, _connection);
                }
                else {
                    await StoreExisting(node, _connection);
                }
                await StoreAdditional(node);
                await tx.CommitAsync();
                if (node.UrlId.HasValue) {
                    _nodeCacheService.Remove(node.UrlId.Value, node.OwnerId);
                }
                if (node.TenantNodes.Any(x => x.UrlPath is not null)) {
                    await _siteDateService.RefreshTenants();
                }
            }
            catch (Exception ex) {
                await tx.RollbackAsync();
                _logger.LogError(ex, $"Error saving {typeof(T)}");
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
