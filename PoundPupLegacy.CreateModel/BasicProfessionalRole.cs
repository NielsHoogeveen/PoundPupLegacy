namespace PoundPupLegacy.DomainModel;

public abstract record BasicProfessionalRole : ProfessionalRole
{
    private BasicProfessionalRole() { }

    public sealed record ToCreateForNewPerson : BasicProfessionalRole, ProfessionalRoleToCreateForNewPerson
    {
        public required Identification.Possible Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ToCreateForExistingPerson {
                Identification = Identification,
                ProfessionalRoleDetails = ProfessionalRoleDetailsForCreate.ResolvePerson(personId)
            };
        }
    }
    public record ToCreateForExistingPerson : BasicProfessionalRole, ProfessionalRoleToCreateForExistingPerson
    {
        public required Identification.Possible Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetails { get; init; }
    }
    public sealed record ToUpdate : BasicProfessionalRole, ProfessionalRoleToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetails { get; init; }
    }
}
