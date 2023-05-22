namespace PoundPupLegacy.CreateModel;

public sealed record Representative : ProfessionalRoleBase, IdentifiableMemberOfCongress
{
    public required int? Id { get; set; }
    public required List<NewHouseTerm> HouseTerms { get; init; }
}