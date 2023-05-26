namespace PoundPupLegacy.CreateModel;

public sealed record NewRepresentativeAsNewPerson : NewProfessionalRoleBaseForNewPerson, EventuallyIdentifiableRepresentative, EventuallyIdentifiableMemberOfCongressForNewPerson
{
    public override EventuallyIdentifiableProfessionalRoleForExistingPerson ResolvePerson(int personId)
    {
        return new NewRepresentativeAsExistingPerson {
            DateTimeRange = DateTimeRange,
            ProfessionId = ProfessionId,
            PersonId = personId,
            HouseTerms = HouseTerms,
            Id = null
        };
    }

    public required List<NewHouseTerm> HouseTerms { get; init; }
}
public sealed record NewRepresentativeAsExistingPerson : NewProfessionalRoleBaseForExistingPerson, EventuallyIdentifiableRepresentative, EventuallyIdentifiableMemberOfCongressForExistingPerson
{
    public required List<NewHouseTerm> HouseTerms { get; init; }
}

public sealed record ExistingRepresentative : ExistingProfessionalRoleBase, ImmediateIdentifiableRepresentative, ImmediatelyIdentifiableMemberOfCongress
{
    public required List<NewHouseTerm> HouseTerms { get; init; }
}

public interface ImmediateIdentifiableRepresentative : Representative, ImmediatelyIdentifiableMemberOfCongress
{

}

public interface EventuallyIdentifiableRepresentative : Representative, EventuallyIdentifiableMemberOfCongress
{

}
public interface Representative: MemberOfCongress
{
}