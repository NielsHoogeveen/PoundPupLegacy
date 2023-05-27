using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

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
                UrlId = null,
                UrlPath = tn.TenantNode!.UrlPath,
                SubgroupId = tn.TenantNode!.SubgroupId,
            }).ToList(),
            Date = viewModel.Date,
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>(),
            TermIds = new List<int>(),
            TypeOfAbuseIds = new List<int>(),
            TypeOfAbuserIds = new List<int>(),
            ChildPlacementTypeId = viewModel.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.DisabilitiesInvolved,
            FamilySizeId = viewModel.FamilySizeId,
            FundamentalFaithInvolved  = viewModel.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.HomeschoolingInvolved,
            Locations = new List<EventuallyIdentifiableLocation>(),
            CaseParties = new List<NewCaseNewCaseParties>(),
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
            NodeTermsToAdd = new List<NodeTermToAdd>(),
            TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
            NodeTermsToRemove = new List<NodeTermToRemove>(),
            TenantNodesToRemove = new List<TenantNodeToDelete>(),
            TenantNodesToUpdate = new List<CreateModel.ExistingTenantNode>(),
            TermsToAdd = new List<NewTermForExistingNameable>(),
            TypeOfAbuseIdsToAdd = new List<int>(),
            TypeOfAbuserIdsToAdd = new List<int>(),
            TypeOfAbuseIdsToRemove = new List<int>(),
            TypeOfAbuserIdsToRemove = new List<int>(),
            ChildPlacementTypeId = viewModel.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.DisabilitiesInvolved,
            FamilySizeId = viewModel.FamilySizeId,
            FundamentalFaithInvolved  = viewModel.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.HomeschoolingInvolved,
            LocationsToDelete = new List<int>(),
            LocationsToUpdate = new List<ImmediatelyIdentifiableLocation>(),
            LocationsToAdd = new List<EventuallyIdentifiableLocation>(),
            CasePartiesToAdd = new List<ExistingCaseNewCaseParties>(),
            CasePartiesToUpdate = new List<ExistingCaseExistingCaseParties>(),
        };
    }
}
