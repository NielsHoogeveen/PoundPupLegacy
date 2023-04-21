using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
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

    public async Task<SubgroupPagedList> GetSubGroupPagedList(int userId, int subgroupId, int pageSize, int pageNumber)
    {
        var offset = (pageNumber - 1) * pageSize;
        try {
            await _connection.OpenAsync();
            await using var reader = await _subgroupsDocumentReaderFactory.CreateAsync(_connection);
            var result = await reader.ReadAsync(new SubgroupsDocumentReaderRequest {
                UserId = userId,
                SubgroupId = subgroupId,
                Limit = pageSize,
                Offset = offset
            });
            if (result != null)
                return result;
            return new SubgroupPagedList {
                Entries = Array.Empty<SubgroupListEntry>(),
                NumberOfEntries = 0
            };
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }

        }
    }
}
