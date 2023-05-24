namespace PoundPupLegacy.CreateModel;

public sealed record Representative : ProfessionalRoleBase, EventuallyIdentifiableMemberOfCongress
{
    public required int? Id { get; set; }
    public required List<NewHouseTerm> HouseTerms { get; init; }
}