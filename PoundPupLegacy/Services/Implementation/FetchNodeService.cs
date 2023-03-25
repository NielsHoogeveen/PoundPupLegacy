using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchNodeService : IFetchNodeService
{
    private readonly NpgsqlConnection _connection;
    public FetchNodeService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Node?> FetchNode(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await NodeDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(urlId, userId, tenantId);
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
