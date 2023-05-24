namespace PoundPupLegacy.CreateModel;

public sealed record Senator : ProfessionalRoleBase, IdentifiableMemberOfCongress
{
    public required int? Id { get; set; }
    public required List<NewSenateTerm> SenateTerms { get; init; }

}
