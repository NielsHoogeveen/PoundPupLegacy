using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchDocumentsService(
    NpgsqlDataSource dataSource,
    ILogger<FetchDocumentsService> logger,
    ISingleItemDatabaseReaderFactory<DocumentsDocumentReaderRequest, Documents> documentsDocumentReaderFactory
) : DatabaseService(dataSource, logger), IFetchDocumentsService
{
    public async Task<Documents> GetArticles(int tenantId, int userId, int[] selectedTerms, int pageNumber, int length)
    {

        var startIndex = (pageNumber - 1) * length;
        return await WithConnection(async (connection) => {
            await using var reader = await documentsDocumentReaderFactory.CreateAsync(connection);
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
        });
    }
}
