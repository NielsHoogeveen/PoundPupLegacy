namespace PoundPupLegacy.CreateModel;

public abstract record Senator : MemberOfCongress
{
    private Senator() { }
    public abstract SenatorDetails SenatorDetails { get; }
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public sealed record SenatorToCreateForNewPerson : Senator, MemberOfCongressToCreateForNewPerson
    {
        public override SenatorDetails SenatorDetails => SenatorDetailsForCreate;
        public required SenatorDetails.SenatorDetailsForCreate SenatorDetailsForCreate { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new SenatorToCreateForExistingPerson {
                IdentificationForCreate = IdentificationForCreate,
                ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId),
                SenatorDetailsToCreate = SenatorDetailsForCreate,
            };
        }
    }
    public record SenatorToCreateForExistingPerson : Senator, MemberOfCongressToCreateForExistingPerson
    {
        public override SenatorDetails SenatorDetails => SenatorDetailsToCreate;
        public required SenatorDetails.SenatorDetailsForCreate SenatorDetailsToCreate { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
    }
    public sealed record SenatorToUpdate : Senator, MemberOfCongressToUpdate
    {
        public override SenatorDetails SenatorDetails => SenatorDetailsForUpdate;
        public required SenatorDetails.SenatorDetailsForUpdate SenatorDetailsForUpdate { get; init; }
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }

        public Identification Identification => IdentificationForUpdate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForUpdate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; init; }
    }
}

public abstract record SenatorDetails
{
    public abstract IEnumerable<SenateTerm> SenateTerms { get; }

    public sealed record SenatorDetailsForCreate : SenatorDetails
    {
        public override IEnumerable<SenateTerm> SenateTerms => SenateTermToCreate;
        public required List<SenateTerm.SenateTermToCreate> SenateTermToCreate { get; init; }
    }
    public sealed record SenatorDetailsForUpdate : SenatorDetails
    {
        public override IEnumerable<SenateTerm> SenateTerms => SenateTermToUpdate;
        public required List<SenateTerm.SenateTermToUpdate> SenateTermToUpdate { get; init; }
    }
}