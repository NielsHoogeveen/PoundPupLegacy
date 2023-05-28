using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal abstract class NodeEditServiceBase<TViewModelEntity, TResolvedViewModelEntity, TExistingViewModel, TNewViewModel, TResolvedNewViewModel, TDomainModel, TCreateModel, TUpdateModel>(
        IDbConnection connection,
        ILogger logger,
        ITenantRefreshService tenantRefreshService,
        IEntityCreatorFactory<TCreateModel> creatorFactory,
        IDatabaseUpdaterFactory<TUpdateModel> updaterFactory,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TNewViewModel> createViewModelReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TExistingViewModel> updateViewModelReaderFactory
    ) : DatabaseService(connection, logger), IEditService<TViewModelEntity, TResolvedViewModelEntity>
    where TViewModelEntity : class, Node
    where TResolvedViewModelEntity : class, TViewModelEntity, ResolvedNode
    where TExistingViewModel : class, ExistingNode, TResolvedViewModelEntity
    where TNewViewModel : class, NewNode, TViewModelEntity
    where TResolvedNewViewModel : class, ResolvedNewNode, TViewModelEntity
    where TDomainModel: class, CreateModel.Node
    where TCreateModel : class, CreateModel.EventuallyIdentifiableNode, TDomainModel
    where TUpdateModel: class, CreateModel.ImmediatelyIdentifiableNode, TDomainModel
{
    protected abstract TUpdateModel Map(TExistingViewModel existingViewModel);
    protected abstract TCreateModel Map(TResolvedNewViewModel newViewModel);
    public async Task<int> SaveAsync(TResolvedViewModelEntity node)
    {
        
        var id = await (node switch {
            TResolvedNewViewModel n => SaveNewAsync(n),
            TExistingViewModel e => SaveExistingAsync(e),
            _ => throw new Exception("Cannot reach")
        });
        await tenantRefreshService.Refresh();
        return id;
    }
    public async Task<TViewModelEntity?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await updateViewModelReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<TViewModelEntity?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createViewModelReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected async Task<int> SaveNewAsync(TResolvedNewViewModel node)
    {

        return await WithTransactedConnection(async (connection) => {
            await using var updater = await creatorFactory.CreateAsync(connection);
            var nodeToCreate = Map(node);
            await updater.CreateAsync(nodeToCreate);
            return nodeToCreate.Id!.Value;
        });
    }
    protected async Task<int> SaveExistingAsync(TExistingViewModel node)
    {
        return await WithTransactedConnection(async (connection) => {
            await using var updater = await updaterFactory.CreateAsync(connection);
            await updater.UpdateAsync(Map(node));
            return node.NodeIdentification.UrlId;
        });
    }
}
