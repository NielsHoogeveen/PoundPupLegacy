using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

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
            NodeTermsToAdd = new List<NodeTermToAdd>(),
            NodeTermsToRemove = new List<NodeTermToRemove>(),
            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
            TenantNodesToRemove = new List<TenantNodeToDelete>(),
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
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            TermIds = new List<int>(),
        };
    }
}