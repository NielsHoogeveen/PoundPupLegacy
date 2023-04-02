using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchCasesService : IFetchCasesService
{
    private NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<CasesDocumentReader> _casesDocumentReaderFactory;

    public FetchCasesService(
        IDbConnection connection,
        IDatabaseReaderFactory<CasesDocumentReader> casesDocumentReaderFactory
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
            return await reader.ReadAsync(new CasesDocumentReader.CasesDocumentRequest {
                Limit = limit,
                Offset = offset,
                TenantId = tenantId,
                UserId = userId,
                CaseType = caseType
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
