using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class SubgroupService : ISubgroupService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<SubgroupsDocumentReaderRequest, SubgroupPagedList> _subgroupsDocumentReaderFactory;

    public SubgroupService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<SubgroupsDocumentReaderRequest, SubgroupPagedList> subgroupsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _subgroupsDocumentReaderFactory = subgroupsDocumentReaderFactory;
    }

    public async Task<SubgroupPagedList?> GetSubGroupPagedList(int userId, int subgroupId, int limit, int offset)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await _subgroupsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new SubgroupsDocumentReaderRequest {
                UserId = userId,
                SubgroupId = subgroupId,
                Limit = limit,
                Offset = offset

            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }

        }
    }
}
