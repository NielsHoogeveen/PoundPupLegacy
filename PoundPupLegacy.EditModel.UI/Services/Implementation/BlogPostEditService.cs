using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class BlogPostEditService(
    IDbConnection connection,
    ILogger<BlogPostEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewBlogPost> createDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingBlogPost> updateDocumentReaderFactory,
    IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest> simpleTextNodeUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITextService textService,
    INodeCreatorFactory<EventuallyIdentifiableBlogPost> blogPostCreatorFactory
) : SimpleTextNodeEditServiceBase<BlogPost, ExistingBlogPost, NewBlogPost, EventuallyIdentifiableBlogPost>(
    connection, 
    logger, 
    tenantRefreshService, 
    simpleTextNodeUpdaterFactory, 
    tagSaveService, 
    tenantNodesSaveService, 
    filesSaveService, 
    textService), IEditService<BlogPost, BlogPost>
{

    protected sealed override INodeCreatorFactory<EventuallyIdentifiableBlogPost> EntityCreatorFactory => blogPostCreatorFactory;

    public async Task<BlogPost?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.BLOG_POST,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }
    public async Task<BlogPost?> GetViewModelAsync(int urlId, int userId, int tenantId)
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

    protected sealed override CreateModel.NewBlogPost Map(NewBlogPost item)
    {
        var now = DateTime.Now;
        return new CreateModel.NewBlogPost {
            Id = null,
            Title = item.Title,
            Text = textService.FormatText(item.Text),
            Teaser = textService.FormatTeaser(item.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.BLOG_POST,
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