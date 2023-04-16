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

    public async Task<Cases> FetchCases(int limit, int offset, int tenantId, int userId, CaseType caseType)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _casesDocumentReaderFactory.CreateAsync(_connection);
            var cases =  await reader.ReadAsync(new CasesDocumentReaderRequest {
                Limit = limit,
                Offset = offset,
                TenantId = tenantId,
                UserId = userId,
                CaseType = caseType
            });
            if(cases is not null)
                return cases;
            return new Cases {
                CaseListEntries = Array.Empty<CaseListEntry>(),
                NumberOfEntries = 0,
            };
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
