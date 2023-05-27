using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;
using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseUpdateMapper(
    ITextService textService,
    IEnumerableMapper<TenantNode, NewTenantNodeForExistingNode> newTenantNodeMapper,
    IEnumerableMapper<TenantNode, ExistingTenantNode> tenantNodeToUpdateMapper,
    IEnumerableMapper<TenantNode, TenantNodeToDelete> tenantNodeToRemoveMapper,
    IEnumerableMapper<Tags, NodeTermToAdd> nodeTermsToAddMapper,
    IEnumerableMapper<Tags, NodeTermToRemove> nodeTermsToRemoveMapper,
    IEnumerableMapper<Location.ExistingLocation, int> locationsToDeleteMapper,
    IEnumerableMapper<Location.ExistingLocation, EventuallyIdentifiableLocation> locationsToAddMapper,
    IEnumerableMapper<Location.ExistingLocation, ImmediatelyIdentifiableLocation> locationsToUpdateMapper,
    IEnumerableMapper<CasePartyTypeCaseParties, ExistingCaseExistingCaseParties> casePartiesUpdateMapper,
    IEnumerableMapper<CasePartyTypeCaseParties, ExistingCaseNewCaseParties> casePartiesAddMapper
    ) : IMapper<ExistingAbuseCase, ImmediatelyIdentifiableAbuseCase>
{
    public ImmediatelyIdentifiableAbuseCase Map(ExistingAbuseCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.ExistingAbuseCase {
            Id = viewModel.NodeId,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = DateTime.Now,
            AuthoringStatusId = 1,
            Date = viewModel.Date,
            FileIdTileImage = null,
            TermsToAdd = new List<NewTermForExistingNameable>(),
            TenantNodesToAdd = newTenantNodeMapper.Map(viewModel.TenantNodes).ToList(),
            TenantNodesToRemove = tenantNodeToRemoveMapper.Map(viewModel.TenantNodes).ToList(),
            TenantNodesToUpdate = tenantNodeToUpdateMapper.Map(viewModel.TenantNodes).ToList(),
            NodeTermsToAdd = nodeTermsToAddMapper.Map(viewModel.Tags).ToList(),
            NodeTermsToRemove = nodeTermsToRemoveMapper.Map(viewModel.Tags).ToList(),
            TypeOfAbuseIdsToAdd = viewModel.TypesOfAbuse.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuserIdsToAdd = viewModel.TypesOfAbuser.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuseIdsToRemove = viewModel.TypesOfAbuse.Where(x => x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuserIdsToRemove = viewModel.TypesOfAbuser.Where(x => x.HasBeenDeleted).Select(x => x.Id).ToList(),
            ChildPlacementTypeId = viewModel.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.DisabilitiesInvolved,
            FamilySizeId = viewModel.FamilySizeId,
            FundamentalFaithInvolved = viewModel.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.HomeschoolingInvolved,
            LocationsToDelete = locationsToDeleteMapper.Map(viewModel.LocationsToUpdate).ToList(),
            LocationsToUpdate = locationsToUpdateMapper.Map(viewModel.LocationsToUpdate).ToList(),
            LocationsToAdd = locationsToAddMapper.Map(viewModel.LocationsToUpdate).ToList(),
            CasePartiesToAdd = casePartiesAddMapper.Map(viewModel.CasePartyTypesCaseParties).ToList(),
            CasePartiesToUpdate = casePartiesUpdateMapper.Map(viewModel.CasePartyTypesCaseParties).ToList(),
        };
    }
}
