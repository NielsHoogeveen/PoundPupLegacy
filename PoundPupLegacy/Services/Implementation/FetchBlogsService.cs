using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;


internal class FetchBlogsService : IFetchBlogsService
{
    private readonly NpgsqlConnection _connection;

    public FetchBlogsService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<List<BlogListEntry>> FetchBlogs(int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await BlogsDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(tenantId);

        }
        finally {
            if(_connection.State == ConnectionState.Open) 
            {
                await _connection.CloseAsync();
            }
            
        }
    }

}
