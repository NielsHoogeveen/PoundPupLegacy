﻿using Npgsql;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchBlogService : IFetchBlogService
{
    private readonly NpgsqlConnection _connection;

    public FetchBlogService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<Blog> FetchBlog(int publisherId, int tenantId, int startIndex, int length)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await BlogDocumentReader.CreateAsync(_connection);
            return await reader.ReadAsync(publisherId, tenantId, startIndex, length);
        }
        finally {
            await _connection.CloseAsync();
        }
    }
    }
