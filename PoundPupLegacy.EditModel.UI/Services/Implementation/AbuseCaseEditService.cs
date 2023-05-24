using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AbuseCaseEditService(
        IDbConnection connection,
        ILogger<AbuseCaseEditService> logger,
        ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewAbuseCase> abuseCaseCreateReaderFactory,
        ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingAbuseCase> abuseCaseUpdateDocumentReaderFactory,
        IDatabaseUpdaterFactory<ImmediatelyIdentifiableAbuseCase> abuseCaseUpdaterFactory,
        ISaveService<IEnumerable<Tag>> tagSaveService,
        ISaveService<IEnumerable<TenantNode>> tenantNodesSaveService,
        ISaveService<IEnumerable<File>> filesSaveService,
        ITenantRefreshService tenantRefreshService,
        INameableCreatorFactory<EventuallyIdentifiableAbuseCase> abuseCaseCreatorFactory,
        ITextService textService
    ) : NodeEditServiceBase<AbuseCase, ExistingAbuseCase, NewAbuseCase, CreateModel.NewAbuseCase>(
        connection,
        logger,
        tagSaveService,
        tenantNodesSaveService,
        filesSaveService,
        tenantRefreshService), IEditService<AbuseCase, AbuseCase>
{
    public async Task<AbuseCase?> GetViewModelAsync(int urlId, int userId, int tenantId)
    {
        return await WithConnection(async (connection) => {
            await using var reader = await abuseCaseUpdateDocumentReaderFactory.CreateAsync(connection);
            var result = await reader.ReadAsync(new NodeUpdateDocumentRequest {
                UrlId = urlId,
                UserId = userId,
                TenantId = tenantId
            });
            return result;
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
        var createDocument = new CreateModel.NewAbuseCase {
            Id = null,
            Title = abuseCase.Title,
            Description = abuseCase.Description is null ? "" : textService.FormatText(abuseCase.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = abuseCase.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = abuseCase.PublisherId,
            TenantNodes = abuseCase.Tenants.Where(t => t.HasTenantNode).Select(tn => new NewTenantNodeForNewNode {
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
            Date = abuseCase.Date,
            FamilySizeId = abuseCase.FamilySizeId,
            FundamentalFaithInvolved = abuseCase.FundamentalFaithInvolved,
            HomeschoolingInvolved = abuseCase.HomeschoolingInvolved,
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName> {
                new  VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = abuseCase.Title,
                    ParentNames = new List<string>(),
                }
            },
            TypeOfAbuseIds = new List<int>(),
            TypeOfAbuserIds = new List<int>(),
            NodeTermIds = new List<int>(),
        };
        await using var abuseCaseCreator = await abuseCaseCreatorFactory.CreateAsync(connection);
        await abuseCaseCreator.CreateAsync(createDocument);
        return createDocument.Id!.Value;
    }

    protected sealed override async Task StoreExisting(ExistingAbuseCase abuseCase, NpgsqlConnection connection)
    {
        await using var updater = await abuseCaseUpdaterFactory.CreateAsync(connection);
        await updater.UpdateAsync(new CreateModel.ExistingAbuseCase {
            Title = abuseCase.Title,
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            TypeOfAbuseIds = new List<int>(),
            TypeOfAbuserIds = new List<int>(),
            NewTenantNodes = abuseCase.TenantNodes.Where(x => !x.Id.HasValue).Select(x => new NewTenantNodeForExistingNode {
                Id = null,
                PublicationStatusId = x.PublicationStatusId,
                TenantId = x.TenantId,
                NodeId = abuseCase.NodeId,
                UrlId = abuseCase.NodeId,
                UrlPath = x.UrlPath,
                SubgroupId = x.SubgroupId,
            }).ToList(),
            TenantNodesToUpdate = abuseCase.TenantNodes.Where(x => x.Id.HasValue).Select(x => new ExistingTenantNode {
                Id = x.Id!.Value,
                PublicationStatusId = x.PublicationStatusId,
                UrlPath = x.UrlPath,
                SubgroupId = x.SubgroupId,
            }).ToList(),
            FileIdTileImage = null,
            VocabularyNames = new List<CreateModel.VocabularyName>(),
            Description = abuseCase.Description is null ? "" : textService.FormatText(abuseCase.Description),
            Id = abuseCase.NodeId,
            Date = abuseCase.Date,
            ChildPlacementTypeId = abuseCase.ChildPlacementTypeId,
            DisabilitiesInvolved = abuseCase.DisabilitiesInvolved,
            FamilySizeId = abuseCase.FamilySizeId,
            FundamentalFaithInvolved = abuseCase.FundamentalFaithInvolved,
            HomeschoolingInvolved = abuseCase.HomeschoolingInvolved,
            NewNodeTerms = new List<NodeTerm>(),
            NodeTermsToRemove = new List<NodeTerm>(),
            TenantNodesToRemove = new List<ExistingTenantNode>()
        }); ;
    }
}
