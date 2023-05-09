using PoundPupLegacy.EditModel.Readers;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class PersonSearchService : SearchService<PersonListItem, PersonsReaderRequest>
{
    public PersonSearchService(
        IDbConnection connection,
        IEnumerableDatabaseReaderFactory<PersonsReaderRequest, PersonListItem> personsReaderFactory): base(connection, personsReaderFactory)
    {
    }
    protected override PersonsReaderRequest GetRequest(int tenantId, string searchString)
    {
        return new PersonsReaderRequest {
            TenantId = tenantId,
            SearchString = searchString

        };
    }
}
