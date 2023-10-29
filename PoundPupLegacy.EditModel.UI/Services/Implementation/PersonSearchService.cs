using Microsoft.Extensions.Logging;
using Npgsql;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PersonSearchService(
    NpgsqlDataSource dataSource,
    ILogger<PersonSearchService> logger,
    IEnumerableDatabaseReaderFactory<PersonsReaderRequest, PersonListItem> personsReaderFactory
) : SearchService<PersonListItem, PersonsReaderRequest>(
    dataSource, 
    logger, 
    personsReaderFactory
)
{
    protected override PersonsReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new PersonsReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
