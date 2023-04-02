using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchNodeService : IFetchNodeService
{
    private readonly NpgsqlConnection _connection;

    private readonly IDatabaseReaderFactory<NodeDocumentReader> _nodeDocumentReaderFactory;
    public FetchNodeService(
        IDbConnection connection,
        IDatabaseReaderFactory<NodeDocumentReader> nodeDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _nodeDocumentReaderFactory = nodeDocumentReaderFactory;
    }

    public async Task<Node?> FetchNode(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _nodeDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeDocumentReader.NodeDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
