using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchCountriesService : IFetchCountriesService
{
    private readonly NpgsqlConnection _connection;

    public FetchCountriesService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<FirstLevelRegionListEntry[]> FetchCountries(int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await CountriesDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(tenantId);
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
