using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;

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
            var reader = await NodeDocumentReader.CreateAsync(_connection);
            var node = await reader.ReadAsync(urlId, userId, tenantId);
            await reader.DisposeAsync();
            return node;
        }
        finally {
            await _connection.CloseAsync();
        }
    }

}
