namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentEditService : NodeEditServiceBase<Document, ExistingDocument, NewDocument, CreateModel.Document>, IEditService<Document>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDocument> _createDocumentReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDocument> _documentUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<DocumentUpdaterRequest> _documentUpdaterFactory;
    private readonly IEntityCreator<CreateModel.Document> _documentCreator;
    private readonly ITextService _textService;


    public DocumentEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDocument> createDocumentReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDocument> documentUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<DocumentUpdaterRequest> documentUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.Document> documentCreator,
        ITextService textService
    ) : base(
        connection,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        tenantRefreshService)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _documentUpdateDocumentReaderFactory = documentUpdateDocumentReaderFactory;
        _createDocumentReaderFactory = createDocumentReaderFactory;
        _documentCreator = documentCreator;
        _textService = textService;
        _documentUpdaterFactory = documentUpdaterFactory;
    }
    public async Task<Document?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _documentUpdateDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
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

    public async Task<Document?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createDocumentReaderFactory.CreateAsync(_connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
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

    protected sealed override async Task<int> StoreNew(NewDocument document, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.Document {
            Id = null,
            Title = document.Title,
            Text = _textService.FormatText(document.Text),
            Teaser = _textService.FormatTeaser(document.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = document.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = document.PublisherId,
            TenantNodes = document.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            PublicationDate = document.Published,
            Documentables = new List<int>(),
            DocumentTypeId = document.DocumentTypeId,
            SourceUrl = document.SourceUrl,
        };
        await _documentCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingDocument document, NpgsqlConnection connection)
    {
        await using var updater = await _documentUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new DocumentUpdaterRequest {
            Title = document.Title,
            Text = _textService.FormatText(document.Text),
            Teaser = _textService.FormatTeaser(document.Text),
            NodeId = document.NodeId,
            SourceUrl = document.SourceUrl,
            DocumentTypeId = document.DocumentTypeId,
            Published = document.Published,
        });

    }
}
