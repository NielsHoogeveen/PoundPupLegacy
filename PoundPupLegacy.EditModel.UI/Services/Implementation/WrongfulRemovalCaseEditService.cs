using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class WrongfulRemovalCaseEditService : NodeEditServiceBase<WrongfulRemovalCase, CreateModel.WrongfulRemovalCase>, IEditService<WrongfulRemovalCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulRemovalCase> _createWrongfulRemovalCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, WrongfulRemovalCase> _wrongfulRemovalCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<WrongfulRemovalCaseUpdaterRequest> _wrongfulRemovalCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.WrongfulRemovalCase> _wrongfulRemovalCaseCreator;
    private readonly ITextService _textService;


    public WrongfulRemovalCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, WrongfulRemovalCase> createWrongfulRemovalCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, WrongfulRemovalCase> wrongfulRemovalCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<WrongfulRemovalCaseUpdaterRequest> wrongfulRemovalCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.WrongfulRemovalCase> wrongfulRemovalCaseCreator,
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
        _wrongfulRemovalCaseUpdateDocumentReaderFactory = wrongfulRemovalCaseUpdateDocumentReaderFactory;
        _createWrongfulRemovalCaseReaderFactory = createWrongfulRemovalCaseReaderFactory;
        _wrongfulRemovalCaseCreator = wrongfulRemovalCaseCreator;
        _textService = textService;
        _wrongfulRemovalCaseUpdaterFactory = wrongfulRemovalCaseUpdaterFactory;
    }
    public async Task<WrongfulRemovalCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _wrongfulRemovalCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<WrongfulRemovalCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createWrongfulRemovalCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task StoreNew(WrongfulRemovalCase wrongfulRemovalCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.WrongfulRemovalCase {
            Id = null,
            Title = wrongfulRemovalCase.Title,
            Description = wrongfulRemovalCase.Description is null? "": _textService.FormatText(wrongfulRemovalCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = wrongfulRemovalCase.OwnerId,
            PublisherId = wrongfulRemovalCase.PublisherId,
            TenantNodes = wrongfulRemovalCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = wrongfulRemovalCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = wrongfulRemovalCase.Title,
                    ParentNames = new List<string>(),
                }
            },
        };
        await _wrongfulRemovalCaseCreator.CreateAsync(createDocument, connection);
        wrongfulRemovalCase.NodeId = createDocument.Id;
    }

    protected sealed override async Task StoreExisting(WrongfulRemovalCase wrongfulRemovalCase, NpgsqlConnection connection)
    {
        await using var updater = await _wrongfulRemovalCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new WrongfulRemovalCaseUpdaterRequest {
            Title = wrongfulRemovalCase.Title,
            Description = wrongfulRemovalCase.Description is null ? "": _textService.FormatText(wrongfulRemovalCase.Description),
            NodeId = wrongfulRemovalCase.NodeId!.Value,
            Date = wrongfulRemovalCase.Date,
        });
    }
}
