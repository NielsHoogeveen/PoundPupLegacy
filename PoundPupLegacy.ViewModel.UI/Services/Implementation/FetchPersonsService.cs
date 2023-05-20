using Microsoft.Extensions.Logging;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.Common.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchPersonsService(
    IDbConnection connection,
    ILogger<FetchPersonsService> logger,
    ISingleItemDatabaseReaderFactory<PersonsDocumentReaderRequest, Persons> personsDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchPersonService
{
    public async Task<Persons> FetchPersons(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption)
    {
        var offset = (pageNumber - 1) * pageSize;

        return await WithConnection(async (connection) => {
            await using var reader = await personsDocumentReaderFactory.CreateAsync(connection);
            var persons = await reader.ReadAsync(new PersonsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
            var result = persons is not null
                ? persons
                : new Persons {
                    Entries = Array.Empty<PersonListEntry>(),
                    NumberOfEntries = 0
                };
            return result;
        });
    }
}
