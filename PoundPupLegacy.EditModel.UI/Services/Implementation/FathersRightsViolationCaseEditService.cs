using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class FathersRightsViolationCaseEditService : NodeEditServiceBase<FathersRightsViolationCase, CreateModel.FathersRightsViolationCase>, IEditService<FathersRightsViolationCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, FathersRightsViolationCase> _createFathersRightsViolationCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, FathersRightsViolationCase> _fathersRightsViolationCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<FathersRightsViolationCaseUpdaterRequest> _fathersRightsViolationCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.FathersRightsViolationCase> _fathersRightsViolationCaseCreator;
    private readonly ITextService _textService;


    public FathersRightsViolationCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, FathersRightsViolationCase> createFathersRightsViolationCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, FathersRightsViolationCase> fathersRightsViolationCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<FathersRightsViolationCaseUpdaterRequest> fathersRightsViolationCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.FathersRightsViolationCase> fathersRightsViolationCaseCreator,
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
        _fathersRightsViolationCaseUpdateDocumentReaderFactory = fathersRightsViolationCaseUpdateDocumentReaderFactory;
        _createFathersRightsViolationCaseReaderFactory = createFathersRightsViolationCaseReaderFactory;
        _fathersRightsViolationCaseCreator = fathersRightsViolationCaseCreator;
        _textService = textService;
        _fathersRightsViolationCaseUpdaterFactory = fathersRightsViolationCaseUpdaterFactory;
    }
    public async Task<FathersRightsViolationCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _fathersRightsViolationCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<FathersRightsViolationCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createFathersRightsViolationCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task StoreNew(FathersRightsViolationCase fathersRightsViolationCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.FathersRightsViolationCase {
            Id = null,
            Title = fathersRightsViolationCase.Title,
            Description = fathersRightsViolationCase.Description is null? "": _textService.FormatText(fathersRightsViolationCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = fathersRightsViolationCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = fathersRightsViolationCase.PublisherId,
            TenantNodes = fathersRightsViolationCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = fathersRightsViolationCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = fathersRightsViolationCase.Title,
                    ParentNames = new List<string>(),
                }
            },
        };
        await _fathersRightsViolationCaseCreator.CreateAsync(createDocument, connection);
        fathersRightsViolationCase.NodeId = createDocument.Id;
    }

    protected sealed override async Task StoreExisting(FathersRightsViolationCase fathersRightsViolationCase, NpgsqlConnection connection)
    {
        await using var updater = await _fathersRightsViolationCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new FathersRightsViolationCaseUpdaterRequest {
            Title = fathersRightsViolationCase.Title,
            Description = fathersRightsViolationCase.Description is null ? "": _textService.FormatText(fathersRightsViolationCase.Description),
            NodeId = fathersRightsViolationCase.NodeId!.Value,
            Date = fathersRightsViolationCase.Date,
        });
    }
}
