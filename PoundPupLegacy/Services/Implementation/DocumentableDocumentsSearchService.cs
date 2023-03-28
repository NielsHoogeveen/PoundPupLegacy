using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;

namespace PoundPupLegacy.Services.Implementation;

public class DocumentableDocumentsSearchService : IDocumentableDocumentsSearchService
{

    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<DocumentableDocumentsDocumentReader> _documentableDocumentsDocumentReaderFactory;


    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

    public DocumentableDocumentsSearchService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<DocumentableDocumentsDocumentReader> documentableDocumentsDocumentReaderFactory)
    {
        _connection = connection;
        _documentableDocumentsDocumentReaderFactory = documentableDocumentsDocumentReaderFactory;
    }

    public async Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, int userId, int tenantId, string str)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        List<DocumentableDocument> tags = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentableDocumentsDocumentReaderFactory.CreateAsync(_connection);
            await foreach(var elem in reader.ReadAsync(new DocumentableDocumentsDocumentReader.DocumentableDocumentsDocumentRequest {
                NodeId = nodeId, 
                UserId = userId, 
                TenantId = tenantId, 
                SearchString = str

            })){
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
