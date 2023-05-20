using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchCountriesService(
    IDbConnection connection,
    ILogger<FetchCountriesService> logger,
    ISingleItemDatabaseReaderFactory<CountriesDocumentReaderRequest, FirstLevelRegionListEntry[]> countriesDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchCountriesService
{
    public async Task<FirstLevelRegionListEntry[]> FetchCountries(int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await countriesDocumentReaderFactory.CreateAsync(connection);
            var countries = await reader.ReadAsync(new CountriesDocumentReaderRequest { TenantId = tenantId });
            if (countries is not null)
                return countries;
            return Array.Empty<FirstLevelRegionListEntry>();
        });
    }
}
