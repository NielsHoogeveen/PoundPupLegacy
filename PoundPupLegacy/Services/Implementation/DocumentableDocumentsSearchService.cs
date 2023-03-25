using Npgsql;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;

namespace PoundPupLegacy.Services.Implementation;

public class DocumentableDocumentsSearchService : IDocumentableDocumentsSearchService
{

    private readonly NpgsqlConnection _connection;

    private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

    public DocumentableDocumentsSearchService(
        NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<DocumentableDocument>> GetDocumentableDocuments(int nodeId, int userId, int tenantId, string str)
    {
        await semaphore.WaitAsync(TimeSpan.FromMilliseconds(100));
        List<DocumentableDocument> tags = new();
        try {
            await _connection.OpenAsync();
            await using var reader = await DocumentableDocumentsDocumentReader.CreateAsync(_connection);
            await foreach(var elem in reader.GetDocumentableDocuments(nodeId, userId, tenantId, str)) {
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
