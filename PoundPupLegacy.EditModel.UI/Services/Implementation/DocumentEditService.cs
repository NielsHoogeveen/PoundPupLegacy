using Npgsql;
using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentEditService : NodeEditServiceBase<Document, CreateModel.Document>, IEditService<Document>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Document> _documentUpdateDocumentReaderFactory;


    public DocumentEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, Document> documentUpdateDocumentReaderFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService

    ) : base(
        connection,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        tenantRefreshService)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _documentUpdateDocumentReaderFactory = documentUpdateDocumentReaderFactory;
    }
    public async Task<Document?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentUpdateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

    public async Task<Document?> GetViewModelAsync(int userId, int tenantId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    protected sealed override async Task StoreNew(Document document, NpgsqlConnection connection)
    {
        await Task.CompletedTask;
    }

    protected sealed override async Task StoreExisting(Document document, NpgsqlConnection connection)
    {
        await Task.CompletedTask;
    }

}
