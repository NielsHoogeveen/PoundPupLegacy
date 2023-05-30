namespace PoundPupLegacy.CreateModel;

public abstract record BasicProfessionalRole : ProfessionalRole
{
    private BasicProfessionalRole() { }
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }

    public sealed record ToCreateForNewPerson : BasicProfessionalRole, ProfessionalRoleToCreateForNewPerson
    {
        public required Identification.Possible IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ToCreateForExistingPerson {
                IdentificationForCreate = IdentificationForCreate,
                ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId)
            };
        }
    }
    public record ToCreateForExistingPerson : BasicProfessionalRole, ProfessionalRoleToCreateForExistingPerson
    {
        public required Identification.Possible IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : BasicProfessionalRole, ProfessionalRoleToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }

        public Identification Identification => IdentificationCertain;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForUpdate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; init; }
    }
}
