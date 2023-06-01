using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

public static class ViewModelRetrieveServiceExtension{
    public static void AddViewModelRetrieveService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>(this IServiceCollection services)
    where TViewModelEntity : class, Node
    where TViewModelCreate : class, TViewModelEntity, NewNode
    where TViewModelUpdate : class, TViewModelEntity, ExistingNode
    {
        services.AddTransient<IViewModelRetrieveService<TViewModelEntity>>(CreateService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>);
    }
    private static ViewModelRetrieveService<TViewModelEntity, TViewModelUpdate, TViewModelCreate> CreateService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>(IServiceProvider serviceProvider)
    where TViewModelEntity : class, Node
    where TViewModelCreate : class, TViewModelEntity, NewNode
    where TViewModelUpdate : class, TViewModelEntity, ExistingNode
    {
        IDbConnection connection = serviceProvider.GetRequiredService<IDbConnection>();
        ILogger logger = serviceProvider.GetRequiredService<ILogger<TViewModelEntity>>();
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TViewModelCreate> createViewModelReaderFactory = serviceProvider.GetRequiredService<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TViewModelCreate>>();
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TViewModelUpdate> updateViewModelReaderFactory = serviceProvider.GetRequiredService<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TViewModelUpdate>>();
        return new ViewModelRetrieveService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>(
            connection,
            logger,
            createViewModelReaderFactory,
            updateViewModelReaderFactory
        );
    }
}

internal class ViewModelRetrieveService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>(
        IDbConnection connection,
        ILogger logger,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TViewModelCreate> createViewModelReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TViewModelUpdate> updateViewModelReaderFactory
    ) : DatabaseService(connection, logger), IViewModelRetrieveService<TViewModelEntity>
    where TViewModelEntity : class, Node
    where TViewModelCreate: class, TViewModelEntity, NewNode
    where TViewModelUpdate: class, TViewModelEntity, ExistingNode
{
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
}
