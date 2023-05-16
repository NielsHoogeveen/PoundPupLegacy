namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AbuseCaseEditService : NodeEditServiceBase<AbuseCase, ExistingAbuseCase, NewAbuseCase, CreateModel.AbuseCase>, IEditService<AbuseCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewAbuseCase> _abuseCaseCreateReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingAbuseCase> _abuseCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<AbuseCaseUpdaterRequest> _abuseCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.AbuseCase> _abuseCaseCreator;
    private readonly ITextService _textService;


    public AbuseCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewAbuseCase> abuseCaseCreateReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingAbuseCase> abuseCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<AbuseCaseUpdaterRequest> abuseCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.AbuseCase> abuseCaseCreator,
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
        _abuseCaseUpdateDocumentReaderFactory = abuseCaseUpdateDocumentReaderFactory;
        _abuseCaseCreateReaderFactory = abuseCaseCreateReaderFactory;
        _abuseCaseCreator = abuseCaseCreator;
        _textService = textService;
        _abuseCaseUpdaterFactory = abuseCaseUpdaterFactory;
    }
    public async Task<AbuseCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<AbuseCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _abuseCaseCreateReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task<int> StoreNew(NewAbuseCase abuseCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.AbuseCase {
            Id = null,
            Title = abuseCase.Title,
            Description = abuseCase.Description is null ? "" : _textService.FormatText(abuseCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = abuseCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = abuseCase.PublisherId,
            TenantNodes = abuseCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            ChildPlacementTypeId = abuseCase.ChildPlacementTypeId,
            DisabilitiesInvolved = abuseCase.DisabilitiesInvolved,
            Date = abuseCase.Date?.ToDateTimeRange(),
            FamilySizeId = abuseCase.FamilySizeId,
            FundamentalFaithInvolved = abuseCase.FundamentalFaithInvolved,
            HomeschoolingInvolved = abuseCase.HomeschoolingInvolved,
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = abuseCase.Title,
                    ParentNames = new List<string>(),
                }
            }
        };
        await _abuseCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingAbuseCase abuseCase, NpgsqlConnection connection)
    {
        await using var updater = await _abuseCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new AbuseCaseUpdaterRequest {
            Title = abuseCase.Title,
            Description = abuseCase.Description is null ? "" : _textService.FormatText(abuseCase.Description),
            NodeId = abuseCase.NodeId,
            Date = abuseCase.Date,
            ChildPlacementTypeId = abuseCase.ChildPlacementTypeId,
            DisabilitiesInvolved = abuseCase.DisabilitiesInvolved,
            FamilySizeId = abuseCase.FamilySizeId,
            FundamentalFaithInvolved = abuseCase.FundamentalFaithInvolved,
            HomeschoolingInvolved = abuseCase.HomeschoolingInvolved,
        });
    }
}
