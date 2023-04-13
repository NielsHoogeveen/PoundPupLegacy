using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchPersonsService : IPersonService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<PersonsDocumentReaderRequest, Persons> _personsDocumentReaderFactory;

    public FetchPersonsService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<PersonsDocumentReaderRequest, Persons> personsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _personsDocumentReaderFactory = personsDocumentReaderFactory;
    }

    public async Task<Persons> FetchPersons(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await _personsDocumentReaderFactory.CreateAsync(_connection);
            var persons =  await reader.ReadAsync(new PersonsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
            if (persons is not null)
                return persons;
            return new Persons {
                Entries = Array.Empty<PersonListEntry>(),
                NumberOfEntries = 0
            };
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
