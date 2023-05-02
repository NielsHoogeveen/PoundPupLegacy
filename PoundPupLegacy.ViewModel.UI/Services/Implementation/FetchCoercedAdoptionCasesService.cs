using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchCoercedAdoptionCasesService : IFetchCoercedAdoptionCasesService
{
    private NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases> _abuseCasesDocumentReaderFactory;

    public FetchCoercedAdoptionCasesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases> abuseCasesDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _abuseCasesDocumentReaderFactory = abuseCasesDocumentReaderFactory;
    }

    public async Task<CoercedAdoptionCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCasesDocumentReaderFactory.CreateAsync(_connection);
            var cases = await reader.ReadAsync(new CoercedAdoptionCasesDocumentReaderRequest {
                Limit = pageSize,
                Offset = startIndex,
                TenantId = tenantId,
                UserId = userId
            });
            var result = cases is not null
                ? cases
                : new CoercedAdoptionCases {
                    Entries = Array.Empty<CaseListEntry>(),
                    NumberOfEntries = 0,
                };

            return result;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
