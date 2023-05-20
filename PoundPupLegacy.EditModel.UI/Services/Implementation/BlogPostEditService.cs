using Microsoft.Extensions.Logging;

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
    IEntityCreator<CreateModel.BlogPost> blogPostCreator
) : SimpleTextNodeEditServiceBase<BlogPost, ExistingBlogPost, NewBlogPost, CreateModel.BlogPost>(
    connection, 
    logger, 
    tenantRefreshService, 
    simpleTextNodeUpdaterFactory, 
    tagSaveService, 
    tenantNodesSaveService, 
    filesSaveService, 
    textService), IEditService<BlogPost>
{

    protected sealed override IEntityCreator<CreateModel.BlogPost> EntityCreator => blogPostCreator;

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

    protected sealed override CreateModel.BlogPost Map(NewBlogPost item)
    {
        var now = DateTime.Now;
        return new CreateModel.BlogPost {
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
            TenantNodes = item.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
        };
    }
}