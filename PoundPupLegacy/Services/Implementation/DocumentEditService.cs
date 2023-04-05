﻿using PoundPupLegacy.EditModel;
using PoundPupLegacy.EditModel.Readers;
using System.Data;
using PoundPupLegacy.Common;
using Npgsql;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class DocumentEditService : NodeEditServiceBase<Document, CreateModel.Document>, IEditService<Document>
{
    private readonly IDatabaseReaderFactory<DocumentUpdateDocumentReader> _documentUpdateDocumentReaderFactory;


    public DocumentEditService(
        IDbConnection connection,
        ISiteDataService siteDataService,
        INodeCacheService nodeCacheService,
        IDatabaseReaderFactory<DocumentUpdateDocumentReader> documentUpdateDocumentReaderFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ILogger<DocumentEditService> logger

    ) : base(
        connection,
        siteDataService,
        nodeCacheService,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        logger)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _documentUpdateDocumentReaderFactory = documentUpdateDocumentReaderFactory;
    }
    public async Task<Document> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentUpdateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeEditDocumentReader.NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }

    public Task<Document> GetViewModelAsync(int userId, int tenantId)
    {
        throw new NotImplementedException();
    }

    protected sealed override async Task StoreNew(Document document, NpgsqlConnection connection)
    {

    }

    protected sealed override async Task StoreExisting(Document document, NpgsqlConnection connection)
    {

    }

}