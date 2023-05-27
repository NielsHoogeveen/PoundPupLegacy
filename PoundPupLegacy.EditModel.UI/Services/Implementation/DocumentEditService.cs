using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentEditService(
    IDbConnection connection,
    ILogger<DocumentEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDocument> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDocument> updateViewModelReaderFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableDocument> updaterFactory,
    IEntityCreatorFactory<EventuallyIdentifiableDocument> creatorFactory,
    ITextService textService
) : NodeEditServiceBase<
    Document,
    Document,
    ExistingDocument,
    NewDocument,
    NewDocument,
    CreateModel.Document,
    EventuallyIdentifiableDocument,
    ImmediatelyIdentifiableDocument>(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<Document, Document>
{

    protected sealed override ImmediatelyIdentifiableDocument Map(ExistingDocument item)
    {
        return new CreateModel.ExistingDocument {
            Id = item.NodeId,
            Title = item.Title,
            Text = textService.FormatText(item.Text),
            Teaser = textService.FormatTeaser(item.Text),
            DocumentTypeId = item.DocumentTypeId,
            Published = item.Published,
            SourceUrl = item.SourceUrl,
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            NodeTermsToAdd = new List<NodeTermToAdd>(),
            NodeTermsToRemove = new List<NodeTermToRemove>(),
            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
            TenantNodesToRemove = new List<TenantNodeToDelete>(),
            TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>()
        };
    }

    protected sealed override EventuallyIdentifiableDocument Map(NewDocument item)
    {
        var now = DateTime.Now;
        return new CreateModel.NewDocument {
            Id = null,
            Title = item.Title,
            Text = textService.FormatText(item.Text),
            Teaser = textService.FormatTeaser(item.Text),
            DocumentTypeId = item.DocumentTypeId,
            Published = item.Published,
            SourceUrl = item.SourceUrl,
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