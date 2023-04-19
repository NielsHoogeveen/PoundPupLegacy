using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;
internal sealed class FetchBlogsService : IFetchBlogsService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>> _blogsDocumentReaderFactory;

    public FetchBlogsService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>> blogsDocumentReaderFactory)
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
            var blogs = await reader.ReadAsync(new BlogsDocumentReaderRequest { TenantId = tenantId });
            if (blogs is not null)
                return blogs;
            return new List<BlogListEntry>();
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
