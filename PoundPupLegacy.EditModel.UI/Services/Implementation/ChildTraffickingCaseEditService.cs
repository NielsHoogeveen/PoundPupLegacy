using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class ChildTraffickingCaseEditService(
    IDbConnection connection,
    ILogger<ChildTraffickingCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, ResolvedNewChildTraffickingCase> createChildTraffickingCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingChildTraffickingCase> childTraffickingCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<ChildTraffickingCaseUpdaterRequest> childTraffickingCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    INameableCreatorFactory<EventuallyIdentifiableChildTraffickingCase> childTraffickingCaseCreatorFactory,
    ITextService textService
) : NodeEditServiceBase<ResolvedChildTraffickingCase, ExistingChildTraffickingCase, ResolvedNewChildTraffickingCase, CreateModel.NewChildTraffickingCase>
(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<ChildTraffickingCase, ResolvedChildTraffickingCase>
{
    public async Task<ChildTraffickingCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await childTraffickingCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<ChildTraffickingCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createChildTraffickingCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(ResolvedNewChildTraffickingCase childTraffickingCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.NewChildTraffickingCase {
            Id = null,
            Title = childTraffickingCase.Title,
            Description = childTraffickingCase.Description is null ? "" : textService.FormatText(childTraffickingCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = childTraffickingCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = childTraffickingCase.PublisherId,
            TenantNodes = childTraffickingCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = childTraffickingCase.Date,
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
            CountryIdFrom = childTraffickingCase.CountryFrom.Id,
            NodeTermIds = new List<int>(),
        };
        await using var childTraffickingCaseCreator = await childTraffickingCaseCreatorFactory.CreateAsync(connection);
        await childTraffickingCaseCreator.CreateAsync(createDocument);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingChildTraffickingCase childTraffickingCase, NpgsqlConnection connection)
    {
        await using var updater = await childTraffickingCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new ChildTraffickingCaseUpdaterRequest {
            Title = childTraffickingCase.Title,
            Description = childTraffickingCase.Description is null ? "" : textService.FormatText(childTraffickingCase.Description),
            NodeId = childTraffickingCase.NodeId,
            Date = childTraffickingCase.Date,
            NumberOfChildrenInvolved = childTraffickingCase.NumberOfChildrenInvolved,
            CountryIdFrom = childTraffickingCase.CountryFrom.Id
        });
    }
}
