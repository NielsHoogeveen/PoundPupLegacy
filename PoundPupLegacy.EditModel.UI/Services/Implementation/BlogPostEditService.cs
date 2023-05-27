using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class BlogPostEditService(
    IDbConnection connection,
    ILogger<BlogPostEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewBlogPost> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingBlogPost> updateViewModelReaderFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableBlogPost> updaterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableBlogPost> creatorFactory,
    ITextService textService
) : NodeEditServiceBase<
    BlogPost, 
    BlogPost,
    ExistingBlogPost, 
    NewBlogPost, 
    NewBlogPost,
    CreateModel.BlogPost,
    EventuallyIdentifiableBlogPost, 
    ImmediatelyIdentifiableBlogPost>(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<BlogPost, BlogPost>
{

    protected sealed override ImmediatelyIdentifiableBlogPost Map(ExistingBlogPost item)
    {
        return new CreateModel.ExistingBlogPost {
            Id = item.NodeId,
            Title = item.Title,
            Text = textService.FormatText(item.Text),
            Teaser = textService.FormatTeaser(item.Text),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            NodeTermsToAdd = new List<NodeTermToAdd>(),
            NodeTermsToRemove = new List<NodeTermToRemove>(),
            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
            TenantNodesToRemove = new List<TenantNodeToDelete>(),
            TenantNodesToUpdate = new List<ExistingTenantNode>()
        };
    }

    protected sealed override EventuallyIdentifiableBlogPost Map(NewBlogPost item)
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
            TenantNodes = item.Tenants.Where(t => t.HasTenantNode).Select(tn => new NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            TermIds = new List<int>(),
        };
    }
}