namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class WrongfulMedicationCaseEditService : NodeEditServiceBase<WrongfulMedicationCase, ExistingWrongfulMedicationCase, NewWrongfulMedicationCase, CreateModel.WrongfulMedicationCase>, IEditService<WrongfulMedicationCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulMedicationCase> _createWrongfulMedicationCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulMedicationCase> _wrongfulMedicationCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<WrongfulMedicationCaseUpdaterRequest> _wrongfulMedicationCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.WrongfulMedicationCase> _wrongfulMedicationCaseCreator;
    private readonly ITextService _textService;


    public WrongfulMedicationCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulMedicationCase> createWrongfulMedicationCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulMedicationCase> wrongfulMedicationCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<WrongfulMedicationCaseUpdaterRequest> wrongfulMedicationCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.WrongfulMedicationCase> wrongfulMedicationCaseCreator,
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
        _wrongfulMedicationCaseUpdateDocumentReaderFactory = wrongfulMedicationCaseUpdateDocumentReaderFactory;
        _createWrongfulMedicationCaseReaderFactory = createWrongfulMedicationCaseReaderFactory;
        _wrongfulMedicationCaseCreator = wrongfulMedicationCaseCreator;
        _textService = textService;
        _wrongfulMedicationCaseUpdaterFactory = wrongfulMedicationCaseUpdaterFactory;
    }
    public async Task<WrongfulMedicationCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _wrongfulMedicationCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<WrongfulMedicationCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createWrongfulMedicationCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task<int> StoreNew(NewWrongfulMedicationCase wrongfulMedicationCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.WrongfulMedicationCase {
            Id = null,
            Title = wrongfulMedicationCase.Title,
            Description = wrongfulMedicationCase.Description is null ? "" : _textService.FormatText(wrongfulMedicationCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = wrongfulMedicationCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = wrongfulMedicationCase.PublisherId,
            TenantNodes = wrongfulMedicationCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = wrongfulMedicationCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = wrongfulMedicationCase.Title,
                    ParentNames = new List<string>(),
                }
            },
        };
        await _wrongfulMedicationCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingWrongfulMedicationCase wrongfulMedicationCase, NpgsqlConnection connection)
    {
        await using var updater = await _wrongfulMedicationCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new WrongfulMedicationCaseUpdaterRequest {
            Title = wrongfulMedicationCase.Title,
            Description = wrongfulMedicationCase.Description is null ? "" : _textService.FormatText(wrongfulMedicationCase.Description),
            NodeId = wrongfulMedicationCase.NodeId,
            Date = wrongfulMedicationCase.Date,
        });
    }
}
