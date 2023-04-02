﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchBlogService : IFetchBlogService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<BlogDocumentReader> _blogDocumentReaderFactory;

    public FetchBlogService(
        IDbConnection connection,
        IDatabaseReaderFactory<BlogDocumentReader> blogDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

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
