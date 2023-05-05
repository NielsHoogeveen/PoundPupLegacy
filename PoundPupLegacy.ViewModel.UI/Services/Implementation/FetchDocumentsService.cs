using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchDocumentsService : IFetchDocumentsService
{
    private readonly NpgsqlConnection _connection;

    public readonly ISingleItemDatabaseReaderFactory<DocumentsDocumentReaderRequest, Documents> _documentsDocumentReaderFactory;
    public FetchDocumentsService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<DocumentsDocumentReaderRequest, Documents> documentsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _documentsDocumentReaderFactory = documentsDocumentReaderFactory;
    }

    public async Task<Documents> GetArticles(int tenantId, int userId, int[] selectedTerms, int pageNumber, int length)
    {

        var startIndex = (pageNumber - 1) * length;
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentsDocumentReaderFactory.CreateAsync(_connection);
            var articles = await reader.ReadAsync(new DocumentsDocumentReaderRequest {
                TenantId = tenantId,
                UserId = userId,
                SelectedTerms = selectedTerms,
                StartIndex = startIndex,
                Length = length
            });
            var result = articles is not null ? articles : new Documents {
                TermNames = Array.Empty<SelectionItem>(),
                Items = new ArticleList {
                    Entries = Array.Empty<ArticleListEntry>(),
                    NumberOfEntries = 0
                }
            };
            return result;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

}
