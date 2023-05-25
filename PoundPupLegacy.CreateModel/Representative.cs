namespace PoundPupLegacy.CreateModel;

public sealed record NewRepresentativeAsNewPerson : NewProfessionalRoleBaseForNewPerson, EventuallyIdentifiableMemberOfCongressForNewPerson
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
public sealed record NewRepresentativeAsExistingPerson : NewProfessionalRoleBaseForExistingPerson, EventuallyIdentifiableMemberOfCongressForExistingPerson
{
    public required List<NewHouseTerm> HouseTerms { get; init; }
}

public sealed record ExistingRepresentative : ExistingProfessionalRoleBase, ImmediatelyIdentifiableMemberOfCongress
{
    public required List<NewHouseTerm> HouseTerms { get; init; }
}