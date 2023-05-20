using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class CoercedAdoptionCaseEditService(
    IDbConnection connection,
    ILogger<CoercedAdoptionCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewCoercedAdoptionCase> createCoercedAdoptionCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingCoercedAdoptionCase> coercedAdoptionCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<CoercedAdoptionCaseUpdaterRequest> coercedAdoptionCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    IEntityCreator<CreateModel.CoercedAdoptionCase> coercedAdoptionCaseCreator,
    ITextService textService
) : NodeEditServiceBase<CoercedAdoptionCase, ExistingCoercedAdoptionCase, NewCoercedAdoptionCase, CreateModel.CoercedAdoptionCase>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<CoercedAdoptionCase>
{
    public async Task<CoercedAdoptionCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await coercedAdoptionCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<CoercedAdoptionCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createCoercedAdoptionCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewCoercedAdoptionCase coercedAdoptionCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.CoercedAdoptionCase {
            Id = null,
            Title = coercedAdoptionCase.Title,
            Description = coercedAdoptionCase.Description is null ? "" : textService.FormatText(coercedAdoptionCase.Description),
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
        await coercedAdoptionCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingCoercedAdoptionCase coercedAdoptionCase, NpgsqlConnection connection)
    {
        await using var updater = await coercedAdoptionCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new CoercedAdoptionCaseUpdaterRequest {
            Title = coercedAdoptionCase.Title,
            Description = coercedAdoptionCase.Description is null ? "" : textService.FormatText(coercedAdoptionCase.Description),
            NodeId = coercedAdoptionCase.NodeId,
            Date = coercedAdoptionCase.Date,
        });
    }
}
