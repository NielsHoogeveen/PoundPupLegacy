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
            return await reader.ReadAsync(userId, tenantId, limit, offset);
        }
        finally {
            if (_connection.State == ConnectionState.Open) 
            {
                await _connection.CloseAsync();
            }
        }
    }

}
