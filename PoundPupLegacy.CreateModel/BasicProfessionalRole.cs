namespace PoundPupLegacy.CreateModel;

public sealed record NewBasicProfessionalRoleForNewPerson : NewProfessionalRoleBaseForNewPerson
{
    public override EventuallyIdentifiableProfessionalRoleForExistingPerson ResolvePerson(int personId)
    {
        return new NewBasicProfessionalRoleForExistingPerson {
            DateTimeRange = DateTimeRange,
            ProfessionId = ProfessionId,
            PersonId = personId,
            Id = null
        };
    }
}
public sealed record NewBasicProfessionalRoleForExistingPerson : NewProfessionalRoleBaseForExistingPerson
{
}
public sealed record ExistingBasicProfessionalRole : ExistingProfessionalRoleBase
{
}
