namespace PoundPupLegacy.CreateModel;

public abstract record BasicProfessionalRole : ProfessionalRole
{
    private BasicProfessionalRole() { }
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public sealed record BasticProfessionalRoleToCreateForNewPerson : BasicProfessionalRole, ProfessionalRoleToCreateForNewPerson
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new BasicProfessionalRoleToCreateForExistingPerson {
                IdentificationForCreate = IdentificationForCreate,
                ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId)
            };
        }
    }
    public record BasicProfessionalRoleToCreateForExistingPerson : BasicProfessionalRole, ProfessionalRoleToCreateForExistingPerson
    {
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
    }
    public sealed record BasicProfessionalRoleToUpdate : BasicProfessionalRole, ProfessionalRoleToUpdate
    {
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }

        public Identification Identification => IdentificationForUpdate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForUpdate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; init; }
    }
}
