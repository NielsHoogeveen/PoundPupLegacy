using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;
using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseUpdateMapper(
    ITextService textService,
    IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, NewTenantNodeForExistingNode> newTenantNodeMapper,
    IEnumerableMapper<TenantNode.ExistingTenantNode, ExistingTenantNode> tenantNodeToUpdateMapper,
    IEnumerableMapper<TenantNode.ExistingTenantNode, TenantNodeToDelete> tenantNodeToRemoveMapper,
    IEnumerableMapper<Tags, NodeTermToAdd> nodeTermsToAddMapper,
    IEnumerableMapper<Tags, NodeTermToRemove> nodeTermsToRemoveMapper,
    IEnumerableMapper<Location.ExistingLocation, int> locationsToDeleteMapper,
    IEnumerableMapper<Location.ExistingLocation, EventuallyIdentifiableLocation> locationsToAddMapper,
    IEnumerableMapper<Location.ExistingLocation, ImmediatelyIdentifiableLocation> locationsToUpdateMapper,
    IEnumerableMapper<CasePartyTypeCaseParties, ExistingCaseExistingCaseParties> casePartiesUpdateMapper,
    IEnumerableMapper<CasePartyTypeCaseParties, ExistingCaseNewCaseParties> casePartiesAddMapper
    ) : IMapper<AbuseCase.ExistingAbuseCase, ImmediatelyIdentifiableAbuseCase>
{
    public ImmediatelyIdentifiableAbuseCase Map(AbuseCase.ExistingAbuseCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.ExistingAbuseCase {
            Id = viewModel.NodeIdentification.NodeId,
            Title = viewModel.NodeDetails.Title,
            Description = viewModel.NameableDetails.Description is null ? "" : textService.FormatText(viewModel.NameableDetails.Description),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            Date = viewModel.CaseDetails.Date,
            FileIdTileImage = null,
            TermsToAdd = new List<NewTermForExistingNameable>(),
            TenantNodesToAdd = newTenantNodeMapper.Map(viewModel.ExistingTenantNodeDetails.TenantNodesToAdd).ToList(),
            TenantNodesToRemove = tenantNodeToRemoveMapper.Map(viewModel.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
            TenantNodesToUpdate = tenantNodeToUpdateMapper.Map(viewModel.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
            NodeTermsToAdd = nodeTermsToAddMapper.Map(viewModel.NodeDetails.Tags).ToList(),
            NodeTermsToRemove = nodeTermsToRemoveMapper.Map(viewModel.NodeDetails.Tags).ToList(),
            TypeOfAbuseIdsToAdd = viewModel.AbuseCaseDetails.TypesOfAbuse.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuserIdsToAdd = viewModel.AbuseCaseDetails.TypesOfAbuser.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuseIdsToRemove = viewModel.AbuseCaseDetails.TypesOfAbuse.Where(x => x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuserIdsToRemove = viewModel.AbuseCaseDetails.TypesOfAbuser.Where(x => x.HasBeenDeleted).Select(x => x.Id).ToList(),
            ChildPlacementTypeId = viewModel.AbuseCaseDetails.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.AbuseCaseDetails.DisabilitiesInvolved,
            FamilySizeId = viewModel.AbuseCaseDetails.FamilySizeId,
            FundamentalFaithInvolved = viewModel.AbuseCaseDetails.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.AbuseCaseDetails.HomeschoolingInvolved,
            LocationsToDelete = locationsToDeleteMapper.Map(viewModel.ExistingLocatableDetails.LocationsToUpdate).ToList(),
            LocationsToUpdate = locationsToUpdateMapper.Map(viewModel.ExistingLocatableDetails.LocationsToUpdate).ToList(),
            LocationsToAdd = locationsToAddMapper.Map(viewModel.ExistingLocatableDetails.LocationsToUpdate).ToList(),
            CasePartiesToAdd = casePartiesAddMapper.Map(viewModel.CaseDetails.CasePartyTypesCaseParties).ToList(),
            CasePartiesToUpdate = casePartiesUpdateMapper.Map(viewModel.CaseDetails.CasePartyTypesCaseParties).ToList(),
        };
    }
}
