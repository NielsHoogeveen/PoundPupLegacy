using PoundPupLegacy.CreateModel;
using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbusCaseCreateMapper(
    ITextService textService,
    IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, NewTenantNodeForNewNode> tenantNodeMapper,
    IEnumerableMapper<Tags, int> termIdsMapper,
    IEnumerableMapper<Location.NewLocation, EventuallyIdentifiableLocation> locationMapper,
    IEnumerableMapper<CasePartyTypeCaseParties, NewCaseNewCaseParties> casePartiesAddMapper
) : IMapper<NewAbuseCase, EventuallyIdentifiableAbuseCase>
{
    public EventuallyIdentifiableAbuseCase Map(NewAbuseCase viewModel)
    {
        var now = DateTime.Now;
        return new CreateModel.NewAbuseCase {
            Id = null,
            Title = viewModel.Title,
            Description = viewModel.Description is null ? "" : textService.FormatText(viewModel.Description),
            ChangedDateTime = now,
            CreatedDateTime = now,
            NodeTypeId = Constants.ABUSE_CASE,
            OwnerId = viewModel.OwnerId,
            AuthoringStatusId = 1,
            PublisherId = viewModel.PublisherId,
            TenantNodes = tenantNodeMapper.Map(viewModel.TenantNodesToAdd).ToList(),
            Date = viewModel.Date,
            FileIdTileImage = null,
            Terms = new List<NewTermForNewNameable>{
                new NewTermForNewNameable
                {
                    Id = null,
                    Name = viewModel.Title,
                    ParentTermIds = new List<int>(),
                    VocabularyId = 0
                } 
            },
            TermIds = termIdsMapper.Map(viewModel.Tags).ToList(),
            TypeOfAbuseIds = viewModel.TypesOfAbuse.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            TypeOfAbuserIds = viewModel.TypesOfAbuser.Where(x => !x.HasBeenDeleted).Select(x => x.Id).ToList(),
            ChildPlacementTypeId = viewModel.ChildPlacementTypeId,
            DisabilitiesInvolved = viewModel.DisabilitiesInvolved,
            FamilySizeId = viewModel.FamilySizeId,
            FundamentalFaithInvolved = viewModel.FundamentalFaithInvolved,
            HomeschoolingInvolved = viewModel.HomeschoolingInvolved,
            Locations = locationMapper.Map(viewModel.LocationsToAdd).ToList(),
            CaseParties = casePartiesAddMapper.Map(viewModel.CasePartyTypesCaseParties).ToList(),
        };
    }
}
