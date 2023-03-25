using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchCasesService : IFetchCasesService
{
    private NpgsqlConnection _connection;

    public FetchCasesService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Cases> FetchCases(int limit, int offset, int tenantId, int userId, CaseType caseType)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await CasesDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(limit, offset, tenantId, userId, caseType);
        }
        finally {
            if (_connection.State == ConnectionState.Open) 
            {
                await _connection.CloseAsync();
            }
        }
    }
}
