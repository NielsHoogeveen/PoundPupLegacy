using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchArticlesService : IFetchArticlesService
{
    private readonly NpgsqlConnection _connection;

    public readonly IDatabaseReaderFactory<ArticlesDocumentReader> _articlesDocumentReaderFactory;
    public FetchArticlesService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<ArticlesDocumentReader> articlesDocumentReaderFactory)
    {
        _connection = connection;
        _articlesDocumentReaderFactory = articlesDocumentReaderFactory;
    }

    public async Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int startIndex, int length)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _articlesDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new ArticlesDocumentReader.ArticlesDocumentRequest {
                TenantId = tenantId,
                SelectedTerms = selectedTerms,
                StartIndex = startIndex,
                Length = length
            });

        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

}
