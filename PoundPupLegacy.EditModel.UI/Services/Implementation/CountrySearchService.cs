using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class CountrySearchService(
    IDbConnection connection,
    ILogger<CountrySearchService> logger,
    IEnumerableDatabaseReaderFactory<CountriesReaderRequest, CountryListItem> readerFactory
) : SearchService<CountryListItem, CountriesReaderRequest>(
    connection, 
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
