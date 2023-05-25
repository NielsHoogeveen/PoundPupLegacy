using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AbuseCaseEditService(
    IDbConnection connection,
    ILogger<AbuseCaseEditService> logger,
    ITenantRefreshService tenantRefreshService,
    ISingleItemDatabaseReaderFactory<NodeCreateDocumentRequest, NewAbuseCase> createViewModelReaderFactory,
    ISingleItemDatabaseReaderFactory<NodeUpdateDocumentRequest, ExistingAbuseCase> updateViewModelReaderFactory,
    IEntityCreatorFactory<EventuallyIdentifiableAbuseCase> creatorFactory,
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableAbuseCase> updaterFactory,
    ITextService textService
) : NodeEditServiceBase<
        EditModel.AbuseCase,
        AbuseCase,
        ExistingAbuseCase,
        NewAbuseCase,
        NewAbuseCase,
        CreateModel.AbuseCase,
        EventuallyIdentifiableAbuseCase,
        ImmediatelyIdentifiableAbuseCase>
(
    connection,
    logger,
    tenantRefreshService,
    creatorFactory,
    updaterFactory,
    createViewModelReaderFactory,
    updateViewModelReaderFactory
), IEditService<AbuseCase, AbuseCase>
{

    protected sealed override EventuallyIdentifiableAbuseCase Map(NewAbuseCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewAbuseCase {
            Id = null,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.DOCUMENT,
            OwnerId = viewModel.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = viewModel.PublisherId,
            TenantNodes = viewModel.Tenants.Where(t => t.HasTenantNode).Select(tn => new CreateModel.NewTenantNodeForNewNode {
                Id = null,
                PublicationStatusId = tn.TenantNode!.PublicationStatusId,
                TenantId = tn.TenantNode!.TenantId,
                NodeId = null,
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = viewModel.Date,
            FileIdTileImage = null,
            VocabularyNames = new List<VocabularyName> {
                new  VocabularyName {
                    OwnerId = Constants.OWNER_SYSTEM,
                    Name = Constants.VOCABULARY_TOPICS,
                    TermName = viewModel.Title,
                    ParentNames = new List<string>(),
                }
            },
            NodeTermIds = new List<int>(),
            TypeOfAbuseIds = new List<int>(),
            TypeOfAbuserIds = new List<int>(),
            ChildPlacementTypeId = viewModel.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.DisabilitiesInvolved,
            FamilySizeId = viewModel.FamilySizeId,
            FundamentalFaithInvolved  = viewModel.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.HomeschoolingInvolved
        };
    }

    protected sealed override ImmediatelyIdentifiableAbuseCase Map(ExistingAbuseCase viewModel)
    {
        return new CreateModel.ExistingAbuseCase {
            Id = viewModel.NodeId,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            Date = viewModel.Date,
            FileIdTileImage = null,
            NewNodeTerms = new List<NodeTerm>(),
            NewTenantNodes = new List<NewTenantNodeForExistingNode>(),
            NodeTermsToRemove = new List<NodeTerm>(),
            TenantNodesToRemove = new List<ExistingTenantNode>(),
            TenantNodesToUpdate = new List<ExistingTenantNode>(),
            VocabularyNames = new List<VocabularyName>(),
            TypeOfAbuseIds = viewModel.TypeOfAbuseIds,
            TypeOfAbuserIds = viewModel.TypeOfAbuserIds,
            ChildPlacementTypeId = viewModel.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.DisabilitiesInvolved,
            FamilySizeId = viewModel.FamilySizeId,
            FundamentalFaithInvolved  = viewModel.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.HomeschoolingInvolved

        };
    }
}
