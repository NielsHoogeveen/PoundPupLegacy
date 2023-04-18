using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FetchBlogService : IFetchBlogService
{
    private readonly NpgsqlConnection _connection;
    private readonly ISingleItemDatabaseReaderFactory<BlogDocumentReaderRequest, Blog> _blogDocumentReaderFactory;

    public FetchBlogService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<BlogDocumentReaderRequest, Blog> blogDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _blogDocumentReaderFactory = blogDocumentReaderFactory;
    }

    public async Task<Blog?> FetchBlog(int publisherId, int tenantId, int startIndex, int length)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _blogDocumentReaderFactory.CreateAsync(_connection);
            var blog = await reader.ReadAsync(new BlogDocumentReaderRequest {
                PublisherId = publisherId,
                TenantId = tenantId,
                StartIndex = startIndex,
                Length = length
            });
            if (blog is null)
                return null;
            return blog;
        }
        finally {
            await _connection.CloseAsync();
        }
    }
}
