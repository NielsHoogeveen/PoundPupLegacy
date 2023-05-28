using PoundPupLegacy.CreateModel;
using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseCreateMapper(
    ITextService textService,
    IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, NewTenantNodeForNewNode> tenantNodeMapper,
    IEnumerableMapper<Tags, int> termIdsMapper,
    IEnumerableMapper<Location.NewLocation, EventuallyIdentifiableLocation> locationMapper,
    IEnumerableMapper<CasePartyTypeCaseParties, NewCaseNewCaseParties> casePartiesAddMapper
) : IMapper<AbuseCase.NewAbuseCase, EventuallyIdentifiableAbuseCase>
{
    public EventuallyIdentifiableAbuseCase Map(AbuseCase.NewAbuseCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewAbuseCase {
            Id = null,
            Title = viewModel.NodeDetails.Title,
            Description = viewModel.NameableDetails.Description is null ? "" : textService.FormatText(viewModel.NameableDetails.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.ABUSE_CASE,
            OwnerId = viewModel.NodeDetails.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = viewModel.NodeDetails.PublisherId,
            TenantNodes = tenantNodeMapper.Map(viewModel.NewTenantNodeDetails.TenantNodesToAdd).ToList(),
            Date = viewModel.CaseDetails.Date,
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>{
                new NewTermForNewNameable
                {
                    Id = null,
                    Name = viewModel.NodeDetails.Title,
                    ParentTermIds = new List<int>(),
                    VocabularyId = 0
                } 
            },
            TermIds = termIdsMapper.Map(viewModel.NodeDetails.Tags).ToList(),
            TypeOfAbuseIds = viewModel.AbuseCaseDetails.TypesOfAbuse.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuserIds = viewModel.AbuseCaseDetails.TypesOfAbuser.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            ChildPlacementTypeId = viewModel.AbuseCaseDetails.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.AbuseCaseDetails.DisabilitiesInvolved,
            FamilySizeId = viewModel.AbuseCaseDetails.FamilySizeId,
            FundamentalFaithInvolved = viewModel.AbuseCaseDetails.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.AbuseCaseDetails.HomeschoolingInvolved,
            Locations = locationMapper.Map(viewModel.NewLocatableDetails.LocationsToAdd).ToList(),
            CaseParties = casePartiesAddMapper.Map(viewModel.CaseDetails.CasePartyTypesCaseParties).ToList(),
        };
    }
}
