using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchPollsService : IFetchPollsService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<PollsDocumentReaderRequest, Polls> _pollsDocumentReaderFactory;
    public FetchPollsService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<PollsDocumentReaderRequest, Polls> pollsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _pollsDocumentReaderFactory = pollsDocumentReaderFactory;
    }


    public async Task<Polls> GetPolls(int userId, int tenantId, int limit, int pageNumber)
    {
        var offset = (pageNumber - 1) * limit;

        try {
            await _connection.OpenAsync();
            await using var reader = await _pollsDocumentReaderFactory.CreateAsync(_connection);
            var polls = await reader.ReadAsync(new PollsDocumentReaderRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset
            });
            var result = polls is not null ? polls : new Polls {
                Entries = Array.Empty<PollListEntry>(),
                NumberOfEntries = 0
            };
            result.PageNumber = pageNumber;
            result.NumberOfPages = (result.NumberOfEntries / limit) + 1;
            return result;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

}
