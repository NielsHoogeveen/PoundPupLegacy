namespace PoundPupLegacy.CreateModel;

public sealed record NewSenator : ProfessionalRoleBase, Senator
{
    public required int? Id { get; set; }
    public required List<NewSenateTerm> SenateTerms { get; init; }

}
public interface Senator : IdentifiableMemberOfCongress
{
    List<NewSenateTerm> SenateTerms { get; }

}