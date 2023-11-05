using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

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
        NpgsqlDataSource dataSource = serviceProvider.GetRequiredService<NpgsqlDataSource>();
        ILogger logger = serviceProvider.GetRequiredService<ILogger<TViewModelEntity>>();
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TViewModelCreate> createViewModelReaderFactory = serviceProvider.GetRequiredService<ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TViewModelCreate>>();
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TViewModelUpdate> updateViewModelReaderFactory = serviceProvider.GetRequiredService<ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TViewModelUpdate>>();
        return new ViewModelRetrieveService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>(
            dataSource,
            logger,
            createViewModelReaderFactory,
            updateViewModelReaderFactory
        );
    }
}

internal class ViewModelRetrieveService<TViewModelEntity, TViewModelUpdate, TViewModelCreate>(
    NpgsqlDataSource dataSource,
    ILogger logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, TViewModelCreate> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, TViewModelUpdate> updateViewModelReaderFactory
) : DatabaseService(dataSource, logger), IViewModelRetrieveService<TViewModelEntity>
    where TViewModelEntity : class, Node
    where TViewModelCreate: class, TViewModelEntity, NewNode
    where TViewModelUpdate: class, TViewModelEntity, ExistingNode
{
    public async Task<TViewModelEntity?> GetViewModelAsync(int nodeId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await updateViewModelReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                NodeId = nodeId,
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
