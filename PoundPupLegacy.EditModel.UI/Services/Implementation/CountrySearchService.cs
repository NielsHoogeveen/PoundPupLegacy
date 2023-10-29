using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class CountrySearchService(
    NpgsqlDataSource dataSource,
    ILogger<CountrySearchService> logger,
    IEnumerableDatabaseReaderFactory<CountriesReaderRequest, CountryListItem> readerFactory
) : SearchService<CountryListItem, CountriesReaderRequest>(
    dataSource, 
    logger, 
    readerFactory
)
{
    protected override CountriesReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new CountriesReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
