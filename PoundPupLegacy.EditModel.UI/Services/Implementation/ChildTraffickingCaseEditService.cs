using Npgsql;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.EditModel.Readers;
using PoundPupLegacy.EditModel.Updaters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class ChildTraffickingCaseEditService : NodeEditServiceBase<ChildTraffickingCase, CreateModel.ChildTraffickingCase>, IEditService<ChildTraffickingCase>
{
    private readonly ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, ChildTraffickingCase> _createChildTraffickingCaseReaderFactory;
    private readonly ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ChildTraffickingCase> _childTraffickingCaseUpdateDocumentReaderFactory;
    private readonly IDatabaseUpdaterFactory<ChildTraffickingCaseUpdaterRequest> _childTraffickingCaseUpdaterFactory;
    private readonly IEntityCreator<CreateModel.ChildTraffickingCase> _childTraffickingCaseCreator;
    private readonly ITextService _textService;


    public ChildTraffickingCaseEditService(
        IDbConnection connection,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, ChildTraffickingCase> createChildTraffickingCaseReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ChildTraffickingCase> childTraffickingCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<ChildTraffickingCaseUpdaterRequest> childTraffickingCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.ChildTraffickingCase> childTraffickingCaseCreator,
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
        _childTraffickingCaseUpdateDocumentReaderFactory = childTraffickingCaseUpdateDocumentReaderFactory;
        _createChildTraffickingCaseReaderFactory = createChildTraffickingCaseReaderFactory;
        _childTraffickingCaseCreator = childTraffickingCaseCreator;
        _textService = textService;
        _childTraffickingCaseUpdaterFactory = childTraffickingCaseUpdaterFactory;
    }
    public async Task<ChildTraffickingCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _childTraffickingCaseUpdateDocumentReaderFactory.CreateAsync(_connection);
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

    public async Task<ChildTraffickingCase?> GetViewModelAsync(int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            await using var reader = await _createChildTraffickingCaseReaderFactory.CreateAsync(_connection);
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

    protected sealed override async Task StoreNew(ChildTraffickingCase childTraffickingCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.ChildTraffickingCase {
            Id = null,
            Title = childTraffickingCase.Title,
            Description = childTraffickingCase.Description is null? "": _textService.FormatText(childTraffickingCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = childTraffickingCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = childTraffickingCase.PublisherId,
            TenantNodes = childTraffickingCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = childTraffickingCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = childTraffickingCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            NumberOfChildrenInvolved = childTraffickingCase.NumberOfChildrenInvolved,
            CountryIdFrom = childTraffickingCase.CountryIdFrom
        };
        await _childTraffickingCaseCreator.CreateAsync(createDocument, connection);
        childTraffickingCase.NodeId = createDocument.Id;
    }

    protected sealed override async Task StoreExisting(ChildTraffickingCase childTraffickingCase, NpgsqlConnection connection)
    {
        await using var updater = await _childTraffickingCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new ChildTraffickingCaseUpdaterRequest {
            Title = childTraffickingCase.Title,
            Description = childTraffickingCase.Description is null ? "": _textService.FormatText(childTraffickingCase.Description),
            NodeId = childTraffickingCase.NodeId!.Value,
            Date = childTraffickingCase.Date,
            NumberOfChildrenInvolved = childTraffickingCase.NumberOfChildrenInvolved,
            CountryIdFrom = childTraffickingCase.CountryIdFrom
        });
    }
}
