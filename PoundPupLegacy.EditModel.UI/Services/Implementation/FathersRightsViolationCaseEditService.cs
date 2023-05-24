using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class FathersRightsViolationCaseEditService(
    IDbConnection connection,
    ILogger<FathersRightsViolationCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewFathersRightsViolationCase> createFathersRightsViolationCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingFathersRightsViolationCase> fathersRightsViolationCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<FathersRightsViolationCaseUpdaterRequest> fathersRightsViolationCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    INameableCreatorFactory<EventuallyIdentifiableFathersRightsViolationCase> fathersRightsViolationCaseCreatorFactory,
    ITextService textService
) : NodeEditServiceBase<FathersRightsViolationCase, ExistingFathersRightsViolationCase, NewFathersRightsViolationCase, CreateModel.NewFathersRightsViolationCase>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<FathersRightsViolationCase, FathersRightsViolationCase>
{
    public async Task<FathersRightsViolationCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await fathersRightsViolationCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<FathersRightsViolationCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createFathersRightsViolationCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewFathersRightsViolationCase fathersRightsViolationCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.NewFathersRightsViolationCase {
            Id = null,
            Title = fathersRightsViolationCase.Title,
            Description = fathersRightsViolationCase.Description is null ? "" : textService.FormatText(fathersRightsViolationCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = fathersRightsViolationCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = fathersRightsViolationCase.PublisherId,
            TenantNodes = fathersRightsViolationCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = fathersRightsViolationCase.Date,
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = fathersRightsViolationCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        await using var fathersRightsViolationCaseCreator = await fathersRightsViolationCaseCreatorFactory.CreateAsync(connection);
        await fathersRightsViolationCaseCreator.CreateAsync(createDocument);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingFathersRightsViolationCase fathersRightsViolationCase, NpgsqlConnection connection)
    {
        await using var updater = await fathersRightsViolationCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new FathersRightsViolationCaseUpdaterRequest {
            Title = fathersRightsViolationCase.Title,
            Description = fathersRightsViolationCase.Description is null ? "" : textService.FormatText(fathersRightsViolationCase.Description),
            NodeId = fathersRightsViolationCase.NodeId,
            Date = fathersRightsViolationCase.Date,
        });
    }
}
