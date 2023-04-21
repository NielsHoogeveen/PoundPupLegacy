using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class DocumentableDocumentsSearchService : IDocumentableDocumentsSearchService
{

    private readonly NpgsqlConnection _connection;
    private readonly IEnumerableDatabaseReaderFactory<DocumentableDocumentsDocumentReaderRequest, DocumentableDocument> _documentableDocumentsDocumentReaderFactory;


    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

    public DocumentableDocumentsSearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<DocumentableDocumentsDocumentReaderRequest, DocumentableDocument> documentableDocumentsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _documentableDocumentsDocumentReaderFactory = documentableDocumentsDocumentReaderFactory;
    }

    public async Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, int userId, int tenantId, string searchString)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        List<DocumentableDocument> tags = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentableDocumentsDocumentReaderFactory.CreateAsync(_connection);
            var request = new DocumentableDocumentsDocumentReaderRequest {
                NodeId = nodeId,
                UserId = userId,
                TenantId = tenantId,
                SearchString = searchString

            };
            await foreach (var elem in reader.ReadAsync(request)) {
                tags.Add(elem);
            }
            return tags;
        }
        finally {
            await _connection.CloseAsync();
            semaphore.Release();
        }

    }
}
