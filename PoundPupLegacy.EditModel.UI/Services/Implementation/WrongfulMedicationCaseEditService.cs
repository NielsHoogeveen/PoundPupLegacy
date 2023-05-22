using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class WrongfulMedicationCaseEditService(
    IDbConnection connection,
    ILogger<WrongfulMedicationCaseEditService> logger,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewWrongfulMedicationCase> createWrongfulMedicationCaseReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingWrongfulMedicationCase> wrongfulMedicationCaseUpdateDocumentReaderFactory,
    IDatabaseUpdaterFactory<WrongfulMedicationCaseUpdaterRequest> wrongfulMedicationCaseUpdaterFactory,
    ISaveService<IEnumerable<Tag>> tagSaveService,
    ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
    ISaveService<IEnumerable<File>> filesSaveService,
    ITenantRefreshService tenantRefreshService,
    IEntityCreator<CreateModel.NewWrongfulMedicationCase> wrongfulMedicationCaseCreator,
    ITextService textService
) : NodeEditServiceBase<WrongfulMedicationCase, ExistingWrongfulMedicationCase, NewWrongfulMedicationCase, CreateModel.NewWrongfulMedicationCase>(
    connection,
    logger,
    tagSaveService,
    tenantNodesSaveService,
    filesSaveService,
    tenantRefreshService
), IEditService<WrongfulMedicationCase, WrongfulMedicationCase>
{
    public async Task<WrongfulMedicationCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await wrongfulMedicationCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<WrongfulMedicationCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await createWrongfulMedicationCaseReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewWrongfulMedicationCase wrongfulMedicationCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.NewWrongfulMedicationCase {
            Id = null,
            Title = wrongfulMedicationCase.Title,
            Description = wrongfulMedicationCase.Description is null ? "" : textService.FormatText(wrongfulMedicationCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = wrongfulMedicationCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = wrongfulMedicationCase.PublisherId,
            TenantNodes = wrongfulMedicationCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = wrongfulMedicationCase.Date?.ToDateTimeRange(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = wrongfulMedicationCase.Title,
                    ParentNames = new List<string>(),
                }
            },
        };
        await wrongfulMedicationCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingWrongfulMedicationCase wrongfulMedicationCase, NpgsqlConnection connection)
    {
        await using var updater = await wrongfulMedicationCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new WrongfulMedicationCaseUpdaterRequest {
            Title = wrongfulMedicationCase.Title,
            Description = wrongfulMedicationCase.Description is null ? "" : textService.FormatText(wrongfulMedicationCase.Description),
            NodeId = wrongfulMedicationCase.NodeId,
            Date = wrongfulMedicationCase.Date,
        });
    }
}
