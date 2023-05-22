using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class DocumentEditService(
    IDbConnection connection,
    ILogger<DocumentEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewDocument> createDocumentReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingDocument> documentUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<DocumentUpdaterRequest> documentUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    IEntityCreator<CreateModel.NewDocument> documentCreator,
    ITextService textService
) : NodeEditServiceBase<Document, ExistingDocument, NewDocument, CreateModel.NewDocument>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<Document, Document>
{
    public async Task<Document?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await documentUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<Document?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewDocument document, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.NewDocument {
            Id = null,
            Title = document.Title,
            Text = textService.FormatText(document.Text),
            Teaser = textService.FormatTeaser(document.Text),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = document.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = document.PublisherId,
            TenantNodes = document.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            PublicationDate = document.Published,
            Documentables = new List<int>(),
            DocumentTypeId = document.DocumentTypeId,
            SourceUrl = document.SourceUrl,
        };
        await documentCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingDocument document, NpgsqlConnection connection)
    {
        await using var updater = await documentUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new DocumentUpdaterRequest {
            Title = document.Title,
            Text = textService.FormatText(document.Text),
            Teaser = textService.FormatTeaser(document.Text),
            NodeId = document.NodeId,
            SourceUrl = document.SourceUrl,
            DocumentTypeId = document.DocumentTypeId,
            Published = document.Published,
        });

    }
}
