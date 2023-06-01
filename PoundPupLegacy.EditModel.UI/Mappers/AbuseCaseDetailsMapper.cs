namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class AbuseCaseDetailsMapper : IMapper<EditModel.AbuseCaseDetails, CreateModel.AbuseCaseDetails>
{
    public CreateModel.AbuseCaseDetails Map(AbuseCaseDetails source)
    {
        return new CreateModel.AbuseCaseDetails {
            ChildPlacementTypeId = source.ChildPlacementTypeId,
            TypeOfAbuseIds = source.TypesOfAbuse.Select(x => x.Id).ToList(),
            TypeOfAbuserIds = source.TypesOfAbuser.Select(x => x.Id).ToList(),
            DisabilitiesInvolved = source.DisabilitiesInvolved,
            FamilySizeId = source.FamilySizeId,
            FundamentalFaithInvolved = source.FundamentalFaithInvolved,
            HomeschoolingInvolved = source.HomeschoolingInvolved
        };
    }
}
