using Npgsql;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class NodeEditServiceBase<T, TCreate>
    where T : Node
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
    protected abstract Task StoreExisting(T node, NpgsqlConnection connection);
    protected abstract Task StoreNew(T node, NpgsqlConnection connection);
    protected virtual async Task StoreAdditional(T node)
    {
        await _tagSaveService.SaveAsync(node.Tags.SelectMany(x => x.Entries), _connection);
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
                if (node.TenantNodes.Any(x => x.UrlPath is not null)) {
                    await _tenantRefreshService.Refresh();
                }
            }
            catch (Exception ex) {
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
