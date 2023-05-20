using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class ChildTraffickingCaseEditService(
    IDbConnection connection,
    ILogger<ChildTraffickingCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewChildTraffickingCase> createChildTraffickingCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingChildTraffickingCase> childTraffickingCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<ChildTraffickingCaseUpdaterRequest> childTraffickingCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    IEntityCreator<CreateModel.ChildTraffickingCase> childTraffickingCaseCreator,
    ITextService textService
) : NodeEditServiceBase<ChildTraffickingCase, ExistingChildTraffickingCase, NewChildTraffickingCase, CreateModel.ChildTraffickingCase>
(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<ChildTraffickingCase>
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

    protected sealed override async Task<int> StoreNew(NewChildTraffickingCase childTraffickingCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.ChildTraffickingCase {
            Id = null,
            Title = childTraffickingCase.Title,
            Description = childTraffickingCase.Description is null ? "" : textService.FormatText(childTraffickingCase.Description),
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
        await childTraffickingCaseCreator.CreateAsync(createDocument, connection);
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
            CountryIdFrom = childTraffickingCase.CountryIdFrom
        });
    }
}
