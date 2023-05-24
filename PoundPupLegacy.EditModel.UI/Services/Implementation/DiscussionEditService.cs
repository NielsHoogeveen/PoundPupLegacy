using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DiscussionEditService(
    IDbConnection connection,
    ILogger<DiscussionEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDiscussion> createDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDiscussion> updateDocumentReaderFactory,
    IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest> simpleTextNodeUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITextService textService,
    INodeCreatorFactory<EventuallyIdentifiableDiscussion> discussionCreatorFactory
) : SimpleTextNodeEditServiceBase<Discussion, ExistingDiscussion, NewDiscussion, EventuallyIdentifiableDiscussion>(
    connection, 
    logger, 
    tenantRefreshService, 
    simpleTextNodeUpdaterFactory, 
    tagSaveService, 
    tenantNodesSaveService, 
    filesSaveService, 
    textService
), IEditService<Discussion, Discussion>
{

    protected sealed override INodeCreatorFactory<EventuallyIdentifiableDiscussion> EntityCreatorFactory => discussionCreatorFactory;

    public async Task<Discussion?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DISCUSSION,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }
    public async Task<Discussion?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await updateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override CreateModel.NewDiscussion Map(NewDiscussion item)
    {
        var now = DateTime.Now;
        return new CreateModel.NewDiscussion {
            Id = null,
            Title = item.Title,
            Text = textService.FormatText(item.Text),
            Teaser = textService.FormatTeaser(item.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DISCUSSION,
            OwnerId = item.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = item.PublisherId,
            TenantNodes = item.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            NodeTermIds = new List<int>(),
        };
    }
}