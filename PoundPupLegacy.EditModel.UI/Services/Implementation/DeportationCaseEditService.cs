namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DeportationCaseEditService : NodeEditServiceBase<DeportationCase, ExistingDeportationCase, NewDeportationCase, CreateModel.DeportationCase>, IEditService<DeportationCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDeportationCase> _createDeportationCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDeportationCase> _deportationCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<DeportationCaseUpdaterRequest> _deportationCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.DeportationCase> _deportationCaseCreator;
    private readonly ITextService _textService;


    public DeportationCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDeportationCase> createDeportationCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDeportationCase> deportationCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<DeportationCaseUpdaterRequest> deportationCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.DeportationCase> deportationCaseCreator,
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
        _deportationCaseUpdateDocumentReaderFactory = deportationCaseUpdateDocumentReaderFactory;
        _createDeportationCaseReaderFactory = createDeportationCaseReaderFactory;
        _deportationCaseCreator = deportationCaseCreator;
        _textService = textService;
        _deportationCaseUpdaterFactory = deportationCaseUpdaterFactory;
    }
    public async Task<DeportationCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _deportationCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<DeportationCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createDeportationCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task<int> StoreNew(NewDeportationCase deportationCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.DeportationCase {
            Id = null,
            Title = deportationCase.Title,
            Description = deportationCase.Description is null ? "" : _textService.FormatText(deportationCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = deportationCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = deportationCase.PublisherId,
            TenantNodes = deportationCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = deportationCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = deportationCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            SubdivisionIdFrom = deportationCase.SubdivisionIdFrom,
            CountryIdTo = deportationCase.CountryIdTo
        };
        await _deportationCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingDeportationCase deportationCase, NpgsqlConnection connection)
    {
        await using var updater = await _deportationCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new DeportationCaseUpdaterRequest {
            Title = deportationCase.Title,
            Description = deportationCase.Description is null ? "" : _textService.FormatText(deportationCase.Description),
            NodeId = deportationCase.NodeId,
            Date = deportationCase.Date,
            SubdivisionIdFrom = deportationCase.SubdivisionIdFrom,
            CountryIdTo = deportationCase.CountryIdTo
        });
    }
}
