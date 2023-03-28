using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchCountriesService : IFetchCountriesService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<CountriesDocumentReader> _countriesDocumentReaderFactory;

    public FetchCountriesService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<CountriesDocumentReader> countriesDocumentReaderFactory)
    {
        _connection = connection;
        _countriesDocumentReaderFactory = countriesDocumentReaderFactory;
    }

    public async Task<FirstLevelRegionListEntry[]> FetchCountries(int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _countriesDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(tenantId);
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
