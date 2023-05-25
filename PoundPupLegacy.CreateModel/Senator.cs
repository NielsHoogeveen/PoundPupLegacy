namespace PoundPupLegacy.CreateModel;

public sealed record NewSenatorAsNewPerson : NewProfessionalRoleBaseForNewPerson, EventuallyIdentifiableMemberOfCongressForNewPerson
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
public sealed record NewSenatorAsExistingPerson : NewProfessionalRoleBaseForExistingPerson, EventuallyIdentifiableMemberOfCongressForExistingPerson
{
    public required List<NewSenateTerm> SenateTerms { get; init; }

}
public sealed record ExistingSenator : ExistingProfessionalRoleBase, ImmediatelyIdentifiableMemberOfCongress
{
    public required List<NewSenateTerm> SenateTerms { get; init; }

}
