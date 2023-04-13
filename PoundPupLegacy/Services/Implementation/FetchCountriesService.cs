using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchCountriesService : IFetchCountriesService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<CountriesDocumentReaderRequest, FirstLevelRegionListEntry[]> _countriesDocumentReaderFactory;

    public FetchCountriesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<CountriesDocumentReaderRequest, FirstLevelRegionListEntry[]> countriesDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _countriesDocumentReaderFactory = countriesDocumentReaderFactory;
    }

    public async Task<FirstLevelRegionListEntry[]> FetchCountries(int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _countriesDocumentReaderFactory.CreateAsync(_connection);
            var countries = await reader.ReadAsync(new CountriesDocumentReaderRequest { TenantId = tenantId});
            if(countries is not null) 
                return countries;
            return Array.Empty<FirstLevelRegionListEntry>();
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
