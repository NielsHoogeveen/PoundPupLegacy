﻿using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

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
            NewNodeTerms = new List<NodeTerm>(),
            NodeTermsToRemove = new List<NodeTerm>(),
            NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
            TenantNodesToRemove = new List<ExistingTenantNode>(),
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
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            NodeTermIds = new List<int>(),
        };
    }
}