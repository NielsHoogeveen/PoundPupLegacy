namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DisruptedPlacementCaseEditService : NodeEditServiceBase<DisruptedPlacementCase, ExistingDisruptedPlacementCase, NewDisruptedPlacementCase, CreateModel.DisruptedPlacementCase>, IEditService<DisruptedPlacementCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDisruptedPlacementCase> _createDisruptedPlacementCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDisruptedPlacementCase> _disruptedPlacementCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<DisruptedPlacementCaseUpdaterRequest> _disruptedPlacementCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.DisruptedPlacementCase> _disruptedPlacementCaseCreator;
    private readonly ITextService _textService;


    public DisruptedPlacementCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDisruptedPlacementCase> createDisruptedPlacementCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDisruptedPlacementCase> disruptedPlacementCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<DisruptedPlacementCaseUpdaterRequest> disruptedPlacementCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.DisruptedPlacementCase> disruptedPlacementCaseCreator,
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
        _disruptedPlacementCaseUpdateDocumentReaderFactory = disruptedPlacementCaseUpdateDocumentReaderFactory;
        _createDisruptedPlacementCaseReaderFactory = createDisruptedPlacementCaseReaderFactory;
        _disruptedPlacementCaseCreator = disruptedPlacementCaseCreator;
        _textService = textService;
        _disruptedPlacementCaseUpdaterFactory = disruptedPlacementCaseUpdaterFactory;
    }
    public async Task<DisruptedPlacementCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _disruptedPlacementCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<DisruptedPlacementCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createDisruptedPlacementCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task<int> StoreNew(NewDisruptedPlacementCase disruptedPlacementCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.DisruptedPlacementCase {
            Id = null,
            Title = disruptedPlacementCase.Title,
            Description = disruptedPlacementCase.Description is null? "": _textService.FormatText(disruptedPlacementCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = disruptedPlacementCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = disruptedPlacementCase.PublisherId,
            TenantNodes = disruptedPlacementCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = disruptedPlacementCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = disruptedPlacementCase.Title,
                    ParentNames = new List<string>(),
                }
            },
        };
        await _disruptedPlacementCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingDisruptedPlacementCase disruptedPlacementCase, NpgsqlConnection connection)
    {
        await using var updater = await _disruptedPlacementCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new DisruptedPlacementCaseUpdaterRequest {
            Title = disruptedPlacementCase.Title,
            Description = disruptedPlacementCase.Description is null ? "": _textService.FormatText(disruptedPlacementCase.Description),
            NodeId = disruptedPlacementCase.NodeId,
            Date = disruptedPlacementCase.Date,
        });
    }
}
