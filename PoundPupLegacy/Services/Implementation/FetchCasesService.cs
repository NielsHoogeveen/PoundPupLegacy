using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;
internal sealed class FetchCasesService : IFetchCasesService
{
    private NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<CasesDocumentReaderRequest, Cases> _casesDocumentReaderFactory;

    public FetchCasesService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<CasesDocumentReaderRequest, Cases> casesDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _casesDocumentReaderFactory = casesDocumentReaderFactory;
    }

    public async Task<Cases> FetchCases(int pageSize, int pageNumber, int tenantId, int userId, CaseType caseType)
    {
        var startIndex = (pageNumber - 1) * pageSize;

        try {
            await _connection.OpenAsync();
            await using var reader = await _casesDocumentReaderFactory.CreateAsync(_connection);
            var cases = await reader.ReadAsync(new CasesDocumentReaderRequest {
                Limit = pageSize,
                Offset = startIndex,
                TenantId = tenantId,
                UserId = userId,
                CaseType = caseType
            });
            var result = cases is not null
                ? cases
                : new Cases {
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
