using Npgsql;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;
internal sealed class FetchDeportationCasesService : IFetchDeportationCasesService
{
    private NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<DeportationCasesDocumentReaderRequest, DeportationCases> _abuseCasesDocumentReaderFactory;

    public FetchDeportationCasesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<DeportationCasesDocumentReaderRequest, DeportationCases> abuseCasesDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _abuseCasesDocumentReaderFactory = abuseCasesDocumentReaderFactory;
    }

    public async Task<DeportationCases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCasesDocumentReaderFactory.CreateAsync(_connection);
            var cases = await reader.ReadAsync(new DeportationCasesDocumentReaderRequest {
                Limit = pageSize,
                Offset = startIndex,
                TenantId = tenantId,
                UserId = userId
            });
            var result = cases is not null
                ? cases
                : new DeportationCases {
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
