using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchBlogService : IFetchBlogService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<BlogDocumentReader> _blogDocumentReaderFactory;

    public FetchBlogService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<BlogDocumentReader> blogDocumentReaderFactory
        )
    {
        _connection = connection;
        _blogDocumentReaderFactory = blogDocumentReaderFactory;
    }

    public async Task<Blog> FetchBlog(int publisherId, int tenantId, int startIndex, int length)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _blogDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new BlogDocumentReader.BlogDocumentRequest {
                PublisherId = publisherId,
                TenantId = tenantId,
                StartIndex = startIndex,
                Length = length
            });
        }
        finally {
            await _connection.CloseAsync();
        }
    }
}
