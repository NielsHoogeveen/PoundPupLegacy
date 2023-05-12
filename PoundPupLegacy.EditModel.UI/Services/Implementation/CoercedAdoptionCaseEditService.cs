using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class CoercedAdoptionCaseEditService : NodeEditServiceBase<CoercedAdoptionCase, CreateModel.CoercedAdoptionCase>, IEditService<CoercedAdoptionCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, CoercedAdoptionCase> _createCoercedAdoptionCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, CoercedAdoptionCase> _coercedAdoptionCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<CoercedAdoptionCaseUpdaterRequest> _coercedAdoptionCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.CoercedAdoptionCase> _coercedAdoptionCaseCreator;
    private readonly ITextService _textService;


    public CoercedAdoptionCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, CoercedAdoptionCase> createCoercedAdoptionCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, CoercedAdoptionCase> coercedAdoptionCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<CoercedAdoptionCaseUpdaterRequest> coercedAdoptionCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.CoercedAdoptionCase> coercedAdoptionCaseCreator,
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
        _coercedAdoptionCaseUpdateDocumentReaderFactory = coercedAdoptionCaseUpdateDocumentReaderFactory;
        _createCoercedAdoptionCaseReaderFactory = createCoercedAdoptionCaseReaderFactory;
        _coercedAdoptionCaseCreator = coercedAdoptionCaseCreator;
        _textService = textService;
        _coercedAdoptionCaseUpdaterFactory = coercedAdoptionCaseUpdaterFactory;
    }
    public async Task<CoercedAdoptionCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _coercedAdoptionCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<CoercedAdoptionCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createCoercedAdoptionCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task StoreNew(CoercedAdoptionCase coercedAdoptionCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.CoercedAdoptionCase {
            Id = null,
            Title = coercedAdoptionCase.Title,
            Description = coercedAdoptionCase.Description is null? "": _textService.FormatText(coercedAdoptionCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = coercedAdoptionCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = coercedAdoptionCase.PublisherId,
            TenantNodes = coercedAdoptionCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = coercedAdoptionCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = coercedAdoptionCase.Title,
                    ParentNames = new List<string>(),
                }
            },
        };
        await _coercedAdoptionCaseCreator.CreateAsync(createDocument, connection);
        coercedAdoptionCase.NodeId = createDocument.Id;
    }

    protected sealed override async Task StoreExisting(CoercedAdoptionCase coercedAdoptionCase, NpgsqlConnection connection)
    {
        await using var updater = await _coercedAdoptionCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new CoercedAdoptionCaseUpdaterRequest {
            Title = coercedAdoptionCase.Title,
            Description = coercedAdoptionCase.Description is null ? "": _textService.FormatText(coercedAdoptionCase.Description),
            NodeId = coercedAdoptionCase.NodeId!.Value,
            Date = coercedAdoptionCase.Date,
        });
    }
}
