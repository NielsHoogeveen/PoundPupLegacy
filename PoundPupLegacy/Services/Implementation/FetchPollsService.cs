using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchPollsService : IFetchPollsService
{
    private readonly NpgsqlConnection _connection;

    public FetchPollsService(
        NpgsqlConnection connection)
    {
        _connection = connection;
    }


    public async Task<Polls> GetPolls(int userId, int tenantId, int limit, int offset)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await PollsDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(new PollsDocumentReader.PollsDocumentRequest {
                UserId = userId, 
                TenantId = tenantId, 
                Limit = limit, 
                Offset = offset
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) 
            {
                await _connection.CloseAsync();
            }
        }
    }

}
