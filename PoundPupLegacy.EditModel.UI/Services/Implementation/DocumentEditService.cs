using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

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
            Documentables = new List<int>(),
            DocumentTypeId = item.DocumentTypeId,
            Published = item.Published,
            SourceUrl = item.SourceUrl,
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            NewNodeTerms = new List<NodeTerm>(),
            NodeTermsToRemove = new List<NodeTerm>(),
            NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
            TenantNodesToRemove = new List<ExistingTenantNode>(),
            TenantNodesToUpdate = new List<ExistingTenantNode>()
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
            Documentables = new List<int>(),
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
            NodeTermIds = new List<int>(),
        };
    }
}