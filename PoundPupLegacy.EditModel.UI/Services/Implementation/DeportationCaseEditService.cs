using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DeportationCaseEditService(
    IDbConnection connection,
    ILogger<DeportationCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDeportationCase> createDeportationCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDeportationCase> deportationCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<DeportationCaseUpdaterRequest> deportationCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    INameableCreatorFactory<EventuallyIdentifiableDeportationCase> deportationCaseCreatorFactory,
    ITextService textService
) : NodeEditServiceBase<DeportationCase, ExistingDeportationCase, NewDeportationCase, CreateModel.NewDeportationCase>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<DeportationCase, DeportationCase>
{
    public async Task<DeportationCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await deportationCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<DeportationCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createDeportationCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewDeportationCase deportationCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.NewDeportationCase {
            Id = null,
            Title = deportationCase.Title,
            Description = deportationCase.Description is null ? "" : textService.FormatText(deportationCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = deportationCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = deportationCase.PublisherId,
            TenantNodes = deportationCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = deportationCase.Date,
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = deportationCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            SubdivisionIdFrom = deportationCase.SubdivisionFrom?.Id,
            CountryIdTo = deportationCase.CountryTo?.Id,
            NodeTermIds = new List<int>(),
        };
        await using var deportationCaseCreator = await deportationCaseCreatorFactory.CreateAsync(connection);
        await deportationCaseCreator.CreateAsync(createDocument);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingDeportationCase deportationCase, NpgsqlConnection connection)
    {
        await using var updater = await deportationCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new DeportationCaseUpdaterRequest {
            Title = deportationCase.Title,
            Description = deportationCase.Description is null ? "" : textService.FormatText(deportationCase.Description),
            NodeId = deportationCase.NodeId,
            Date = deportationCase.Date,
            SubdivisionIdFrom = deportationCase.SubdivisionFrom?.Id,
            CountryIdTo = deportationCase.CountryTo?.Id
        });
    }
}
