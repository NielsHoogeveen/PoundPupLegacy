﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal class FetchPollsService : IFetchPollsService
{
    private readonly NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<PollsDocumentReader> _pollsDocumentReaderFactory;
    public FetchPollsService(
        IDbConnection connection,
        IDatabaseReaderFactory<PollsDocumentReader> pollsDocumentReaderFactory)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;

        _pollsDocumentReaderFactory = pollsDocumentReaderFactory;
    }


    public async Task<Polls> GetPolls(int userId, int tenantId, int limit, int offset)
    {

        try {
            await _connection.OpenAsync();
            await using var reader = await _pollsDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new PollsDocumentReader.PollsDocumentRequest {
                UserId = userId,
                TenantId = tenantId,
                Limit = limit,
                Offset = offset
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

}
