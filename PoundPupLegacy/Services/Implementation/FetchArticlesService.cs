using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchArticlesService : IFetchArticlesService
{
    private readonly NpgsqlConnection _connection;

    public readonly ISingleItemDatabaseReaderFactory<ArticlesDocumentReaderRequest, Articles> _articlesDocumentReaderFactory;
    public FetchArticlesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<ArticlesDocumentReaderRequest, Articles> articlesDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _articlesDocumentReaderFactory = articlesDocumentReaderFactory;
    }

    public async Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int pageNumber, int length, string termNamePrefix)
    {

        var startIndex = (pageNumber - 1) * length;
        try {
            await _connection.OpenAsync();
            await using var reader = await _articlesDocumentReaderFactory.CreateAsync(_connection);
            var articles = await reader.ReadAsync(new ArticlesDocumentReaderRequest {
                TenantId = tenantId,
                SelectedTerms = selectedTerms,
                StartIndex = startIndex,
                Length = length
            });
            var result = articles is not null ? articles : new Articles {
                Entries = Array.Empty<ArticleListEntry>(),
                NumberOfEntries = 0
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
