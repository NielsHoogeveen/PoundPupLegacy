using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;


internal class FetchBlogsService : IFetchBlogsService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<BlogsDocumentReader> _blogsDocumentReaderFactory;

    public FetchBlogsService(
        IDbConnection connection,
        IDatabaseReaderFactory<BlogsDocumentReader> blogsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _blogsDocumentReaderFactory = blogsDocumentReaderFactory;
    }

    public async Task<List<BlogListEntry>> FetchBlogs(int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _blogsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(tenantId);

        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }

        }
    }

}
