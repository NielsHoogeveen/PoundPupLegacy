using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DiscussionEditService(
    IDbConnection connection,
    ILogger<DiscussionEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDiscussion> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDiscussion> updateViewModelReaderFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableDiscussion> updaterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableDiscussion> creatorFactory,
    ITextService textService
) : NodeEditServiceBase<
    Discussion,
    Discussion,
    ExistingDiscussion,
    NewDiscussion,
    NewDiscussion,
    CreateModel.Discussion,
    EventuallyIdentifiableDiscussion,
    ImmediatelyIdentifiableDiscussion>(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<Discussion, Discussion>
{

    protected sealed override ImmediatelyIdentifiableDiscussion Map(ExistingDiscussion item)
    {
        return new CreateModel.ExistingDiscussion {
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

    protected sealed override EventuallyIdentifiableDiscussion Map(NewDiscussion item)
    {
        var now = DateTime.Now;
        return new CreateModel.NewDiscussion {
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