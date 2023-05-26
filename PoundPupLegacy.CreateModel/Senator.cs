namespace PoundPupLegacy.CreateModel;

public sealed record NewSenatorAsNewPerson : NewProfessionalRoleBaseForNewPerson, EventuallyIdentifiableSenator, EventuallyIdentifiableMemberOfCongressForNewPerson
{
    public required List<NewSenateTerm> SenateTerms { get; init; }
    public override EventuallyIdentifiableProfessionalRoleForExistingPerson ResolvePerson(int personId)
    {
        return new NewSenatorAsExistingPerson {
            DateTimeRange = DateTimeRange,
            ProfessionId = ProfessionId,
            PersonId = personId,
            SenateTerms = SenateTerms,
            Id = null
        };
    }
}
public sealed record NewSenatorAsExistingPerson : NewProfessionalRoleBaseForExistingPerson, EventuallyIdentifiableSenator, EventuallyIdentifiableMemberOfCongressForExistingPerson
{
    public required List<NewSenateTerm> SenateTerms { get; init; }

}
public sealed record ExistingSenator : ExistingProfessionalRoleBase, ImmediatelyIdentifiableSenator, ImmediatelyIdentifiableMemberOfCongress
{
    public required List<NewSenateTerm> SenateTerms { get; init; }

}
public interface ImmediatelyIdentifiableSenator : Senator, ImmediatelyIdentifiableMemberOfCongress
{

}
public interface EventuallyIdentifiableSenator : Senator, EventuallyIdentifiableMemberOfCongress
{

}
public interface Senator: MemberOfCongress
{

}