namespace PoundPupLegacy.EditModel.Mappers;

internal class AbuseCaseDetailsMapper : IMapper<AbuseCaseDetails, DomainModel.AbuseCaseDetails>
{
    public DomainModel.AbuseCaseDetails Map(AbuseCaseDetails source)
    {
        return new DomainModel.AbuseCaseDetails {
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
