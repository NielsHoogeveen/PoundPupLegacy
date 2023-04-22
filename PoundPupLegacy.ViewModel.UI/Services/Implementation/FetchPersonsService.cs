using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.Models.SearchOption;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchPersonsService : IFetchPersonService
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

    public async Task<Persons> FetchPersons(int userId, int tenantId, int pageSize, int pageNumber, string searchTerm, SearchOption searchOption)
    {
        var offset = (pageNumber - 1) * pageSize;

        try
        {
            await _connection.OpenAsync();
            await using var reader = await _personsDocumentReaderFactory.CreateAsync(_connection);
            var persons = await reader.ReadAsync(new PersonsDocumentReaderRequest
            {
                UserId = userId,
                TenantId = tenantId,
                Limit = pageSize,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
            var result = persons is not null
                ? persons
                : new Persons
                {
                    Entries = Array.Empty<PersonListEntry>(),
                    NumberOfEntries = 0
                };
            return result;
        }
        finally
        {
            if (_connection.State == ConnectionState.Open)
            {
                await _connection.CloseAsync();
            }
        }
    }
}
