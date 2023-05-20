using Microsoft.Extensions.Logging;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AbuseCaseEditService(
        IDbConnection connection,
        ILogger<AbuseCaseEditService> logger,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewAbuseCase> abuseCaseCreateReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingAbuseCase> abuseCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<AbuseCaseUpdaterRequest> abuseCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        IEntityCreator<CreateModel.AbuseCase> abuseCaseCreator,
        ITextService textService
    ) : NodeEditServiceBase<AbuseCase, ExistingAbuseCase, NewAbuseCase, CreateModel.AbuseCase>(
        connection,
        logger,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        tenantRefreshService), IEditService<AbuseCase>
{
    public async Task<AbuseCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await abuseCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    public async Task<AbuseCase?> GetViewModelAsync(int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await abuseCaseCreateReaderFactory.CreateAsync(connection);
            return await reader.ReadAsync(new NodeCreateDocumentRequest {
                NodeTypeId = Constants.DOCUMENT,
                UserId = userId,
                TenantId = tenantId
            });
        });
    }

    protected sealed override async Task<int> StoreNew(NewAbuseCase abuseCase, NpgsqlConnection connection)
    {
        var now = DateTime.Now;
        var createDocument = new CreateModel.AbuseCase {
            Id = null,
            Title = abuseCase.Title,
            Description = abuseCase.Description is null ? "" : textService.FormatText(abuseCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = abuseCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = abuseCase.PublisherId,
            TenantNodes = abuseCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.TenantNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            ChildPlacementTypeId = abuseCase.ChildPlacementTypeId,
            DisabilitiesInvolved = abuseCase.DisabilitiesInvolved,
            Date = abuseCase.Date?.ToDateTimeRange(),
            FamilySizeId = abuseCase.FamilySizeId,
            FundamentalFaithInvolved = abuseCase.FundamentalFaithInvolved,
            HomeschoolingInvolved = abuseCase.HomeschoolingInvolved,
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName> {
                new  CreateModel.VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = abuseCase.Title,
                    ParentNames = new List<string>(),
                }
            }
        };
        await abuseCaseCreator.CreateAsync(createDocument, connection);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingAbuseCase abuseCase, NpgsqlConnection connection)
    {
        await using var updater = await abuseCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new AbuseCaseUpdaterRequest {
            Title = abuseCase.Title,
            Description = abuseCase.Description is null ? "" : textService.FormatText(abuseCase.Description),
            NodeId = abuseCase.NodeId,
            Date = abuseCase.Date,
            ChildPlacementTypeId = abuseCase.ChildPlacementTypeId,
            DisabilitiesInvolved = abuseCase.DisabilitiesInvolved,
            FamilySizeId = abuseCase.FamilySizeId,
            FundamentalFaithInvolved = abuseCase.FundamentalFaithInvolved,
            HomeschoolingInvolved = abuseCase.HomeschoolingInvolved,
        });
    }
}
