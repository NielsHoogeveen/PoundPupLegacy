using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class WrongfulRemovalCaseEditService(
    IDbConnection connection,
    ILogger<WrongfulRemovalCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulRemovalCase> createWrongfulRemovalCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulRemovalCase> wrongfulRemovalCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<WrongfulRemovalCaseUpdaterRequest> wrongfulRemovalCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    IEntityCreator<CreateModel.WrongfulRemovalCase> wrongfulRemovalCaseCreator,
    ITextService textService
) : NodeEditServiceBase<WrongfulRemovalCase, ExistingWrongfulRemovalCase, NewWrongfulRemovalCase, CreateModel.WrongfulRemovalCase>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<WrongfulRemovalCase>
{
    public async Task<WrongfulRemovalCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await wrongfulRemovalCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<WrongfulRemovalCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createWrongfulRemovalCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewWrongfulRemovalCase wrongfulRemovalCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.WrongfulRemovalCase {
            Id = null,
            Title = wrongfulRemovalCase.Title,
            Description = wrongfulRemovalCase.Description is null ? "" : textService.FormatText(wrongfulRemovalCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = wrongfulRemovalCase.OwnerId,
            AuthoringStatusId = 1,
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
        await wrongfulRemovalCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingWrongfulRemovalCase wrongfulRemovalCase, NpgsqlConnection connection)
    {
        await using var updater = await wrongfulRemovalCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new WrongfulRemovalCaseUpdaterRequest {
            Title = wrongfulRemovalCase.Title,
            Description = wrongfulRemovalCase.Description is null ? "" : textService.FormatText(wrongfulRemovalCase.Description),
            NodeId = wrongfulRemovalCase.NodeId,
            Date = wrongfulRemovalCase.Date,
        });
    }
}
