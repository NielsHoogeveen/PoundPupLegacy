using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using SearchOption = PoundPupLegacy.ViewModel.SearchOption;

namespace PoundPupLegacy.Services.Implementation;

public class FetchPersonsService : IPersonService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<PersonsDocumentReader> _personsDocumentReaderFactory;

    public FetchPersonsService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<PersonsDocumentReader> personsDocumentReaderFactory)
    {
        _connection = connection;
        _personsDocumentReaderFactory = personsDocumentReaderFactory;
    }

    public async Task<Persons> FetchPersons(int userId, int tenantId, int limit, int offset, string searchTerm, SearchOption searchOption)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await _personsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new PersonsDocumentReader.PersonsDocumentRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset,
                SearchTerm = searchTerm,
                SearchOption = searchOption
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
