using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DisruptedPlacementCaseEditService(
    IDbConnection connection,
    ILogger<DisruptedPlacementCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDisruptedPlacementCase> createDisruptedPlacementCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDisruptedPlacementCase> disruptedPlacementCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<DisruptedPlacementCaseUpdaterRequest> disruptedPlacementCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    INameableCreatorFactory<EventuallyIdentifiableDisruptedPlacementCase> disruptedPlacementCaseCreatorFactory,
    ITextService textService
) : NodeEditServiceBase<DisruptedPlacementCase, ExistingDisruptedPlacementCase, NewDisruptedPlacementCase, CreateModel.NewDisruptedPlacementCase>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<DisruptedPlacementCase, DisruptedPlacementCase>
{
    public async Task<DisruptedPlacementCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await disruptedPlacementCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<DisruptedPlacementCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createDisruptedPlacementCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewDisruptedPlacementCase disruptedPlacementCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.NewDisruptedPlacementCase {
            Id = null,
            Title = disruptedPlacementCase.Title,
            Description = disruptedPlacementCase.Description is null ? "" : textService.FormatText(disruptedPlacementCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = disruptedPlacementCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = disruptedPlacementCase.PublisherId,
            TenantNodes = disruptedPlacementCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = disruptedPlacementCase.Date,
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = disruptedPlacementCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
        };
        await using var disruptedPlacementCaseCreator = await disruptedPlacementCaseCreatorFactory.CreateAsync(connection);
        await disruptedPlacementCaseCreator.CreateAsync(createDocument);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingDisruptedPlacementCase disruptedPlacementCase, NpgsqlConnection connection)
    {
        await using var updater = await disruptedPlacementCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new DisruptedPlacementCaseUpdaterRequest {
            Title = disruptedPlacementCase.Title,
            Description = disruptedPlacementCase.Description is null ? "" : textService.FormatText(disruptedPlacementCase.Description),
            NodeId = disruptedPlacementCase.NodeId,
            Date = disruptedPlacementCase.Date,
        });
    }
}
