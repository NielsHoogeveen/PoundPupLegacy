//using Microsoft.AspNetCore.Mvc.Formatters;
//using PoundPupLegacy.CreateModel;
//using PoundPupLegacy.CreateModel.Deleters;
//using PoundPupLegacy.EditModel.UI.Services;

//namespace PoundPupLegacy.EditModel.UI.Mappers;

//internal class AbusCaseUpdateMapper(
//    ITextService textService,
//    IMapper<EditModel.NodeDetails.NodeDetailsForUpdate, CreateModel.NodeDetails.NodeDetailsForUpdate> nodeDetailsMapper,
//    IEnumerableMapper<TenantNode.NewTenantNodeForExistingNode, TenantNode.NewTenantNodeForExistingNode> newTenantNodeMapper,
//    IEnumerableMapper<TenantNode.ExistingTenantNode, TenantNode.ExistingTenantNode> tenantNodeToUpdateMapper,
//    IEnumerableMapper<TenantNode.ExistingTenantNode, TenantNodeToDelete> tenantNodeToRemoveMapper,
//    IEnumerableMapper<Tags, NodeTermToAdd> nodeTermsToAddMapper,
//    IEnumerableMapper<Tags, NodeTermToRemove> nodeTermsToRemoveMapper,
//    IEnumerableMapper<Location.ExistingLocation, int> locationsToDeleteMapper,
//    IEnumerableMapper<Location.ExistingLocation, EventuallyIdentifiableLocation> locationsToAddMapper,
//    IEnumerableMapper<Location.ExistingLocation, ImmediatelyIdentifiableLocation> locationsToUpdateMapper,
//    IEnumerableMapper<CasePartyTypeCaseParties, CaseExistingCasePartiesToCreate> casePartiesUpdateMapper,
//    IEnumerableMapper<CasePartyTypeCaseParties, CaseNewCasePartiesToUpdate> casePartiesAddMapper
//    ) : IMapper<EditModel.AbuseCase.ExistingAbuseCase, CreateModel.AbuseCase.AbuseCaseToUpdate>
//{
//    public CreateModel.AbuseCase.AbuseCaseToUpdate Map(EditModel.AbuseCase.ExistingAbuseCase viewModel)
//    {
//        var now = DateTime.Now;
//        return new CreateModel.AbuseCase.AbuseCaseToUpdate {
//            IdentificationForUpdate = new Identification.IdentificationForUpdate { 
//                Id = viewModel.NodeIdentification.NodeId
//            },
//            NodeDetailsForUpdate = nodeDetailsMapper.Map(viewModel.NodeDetailsForUpdate),
//            NameableDetailsForUpdate = new CreateModel.NameableDetails.NameableDetailsForUpdate { 
//                Description = viewModel.NameableDetails.Description is null ? "" : textService.FormatText(viewModel.NameableDetails.Description),
//                TermsToAdd = new List<NewTermForExistingNameable>(),
//                FileIdTileImage = null,
//            },
//            CaseDetailsForUpdate = new CreateModel.CaseDetails.CaseDetailsForUpdate {
//                Date = viewModel.CaseDetails.Date,
//            },
//            AbuseCaseDetails = new CreateModel.AbuseCaseDetails { 
//            },
            
//            //TenantNodesToAdd = newTenantNodeMapper.Map(viewModel.ExistingTenantNodeDetails.TenantNodesToAdd).ToList(),
//            //TenantNodesToRemove = tenantNodeToRemoveMapper.Map(viewModel.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
//            //TenantNodesToUpdate = tenantNodeToUpdateMapper.Map(viewModel.ExistingTenantNodeDetails.TenantNodesToUpdate).ToList(),
//            //NodeTermsToAdd = nodeTermsToAddMapper.Map(viewModel.NodeDetails.Tags).ToList(),
//            //NodeTermsToRemove = nodeTermsToRemoveMapper.Map(viewModel.NodeDetails.Tags).ToList(),

//            TypeOfAbuseIdsToAdd = viewModel.AbuseCaseDetails.TypesOfAbuse.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
//            TypeOfAbuserIdsToAdd = viewModel.AbuseCaseDetails.TypesOfAbuser.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
//            TypeOfAbuseIdsToRemove = viewModel.AbuseCaseDetails.TypesOfAbuse.Where(x => x.HasBeenDeleted).Select(x => x.Id).ToList(),
//            TypeOfAbuserIdsToRemove = viewModel.AbuseCaseDetails.TypesOfAbuser.Where(x => x.HasBeenDeleted).Select(x => x.Id).ToList(),
//            ChildPlacementTypeId = viewModel.AbuseCaseDetails.ChildPlacementTypeId,
//            DisabilitiesInvolved = viewModel.AbuseCaseDetails.DisabilitiesInvolved,
//            FamilySizeId = viewModel.AbuseCaseDetails.FamilySizeId,
//            FundamentalFaithInvolved = viewModel.AbuseCaseDetails.FundamentalFaithInvolved,
//            HomeschoolingInvolved = viewModel.AbuseCaseDetails.HomeschoolingInvolved,
//            LocationsToDelete = locationsToDeleteMapper.Map(viewModel.ExistingLocatableDetails.LocationsToUpdate).ToList(),
//            LocationsToUpdate = locationsToUpdateMapper.Map(viewModel.ExistingLocatableDetails.LocationsToUpdate).ToList(),
//            LocationsToAdd = locationsToAddMapper.Map(viewModel.ExistingLocatableDetails.LocationsToUpdate).ToList(),
//            CasePartiesToAdd = casePartiesAddMapper.Map(viewModel.CaseDetails.CasePartyTypesCaseParties).ToList(),
//            CasePartiesToUpdate = casePartiesUpdateMapper.Map(viewModel.CaseDetails.CasePartyTypesCaseParties).ToList(),
//        };
//    }
//}
