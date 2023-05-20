using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PersonSearchService(
    IDbConnection connection,
    ILogger<PersonSearchService> logger,
    IEnumerableDatabaseReaderFactory<PersonsReaderRequest, PersonListItem> personsReaderFactory
) : SearchService<PersonListItem, PersonsReaderRequest>(
    connection, 
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
