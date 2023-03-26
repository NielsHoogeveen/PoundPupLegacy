using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchArticlesService : IFetchArticlesService
{
    private readonly NpgsqlConnection _connection;

    public FetchArticlesService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Articles> GetArticles(int tenantId, List<int> selectedTerms, int startIndex, int length)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await ArticlesDocumentReader.CreateAsync(_connection);
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
