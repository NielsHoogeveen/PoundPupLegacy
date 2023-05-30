namespace PoundPupLegacy.CreateModel;

public abstract record Senator : MemberOfCongress
{
    private Senator() { }
    public abstract SenatorDetails SenatorDetails { get; }
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public sealed record ToCreateForNewPerson : Senator, MemberOfCongressToCreateForNewPerson
    {
        public override SenatorDetails SenatorDetails => SenatorDetailsForCreate;
        public required SenatorDetails.ForCreate SenatorDetailsForCreate { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ToCreateForExistingPerson {
                IdentificationForCreate = IdentificationForCreate,
                ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId),
                SenatorDetailsToCreate = SenatorDetailsForCreate,
            };
        }
    }
    public record ToCreateForExistingPerson : Senator, MemberOfCongressToCreateForExistingPerson
    {
        public override SenatorDetails SenatorDetails => SenatorDetailsToCreate;
        public required SenatorDetails.ForCreate SenatorDetailsToCreate { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
    }
    public sealed record SenatorToUpdate : Senator, MemberOfCongressToUpdate
    {
        public override SenatorDetails SenatorDetails => SenatorDetailsForUpdate;
        public required SenatorDetails.ForUpdate SenatorDetailsForUpdate { get; init; }
        public required Identification.Certain IdentificationCertain { get; init; }
        public Identification Identification => IdentificationCertain;
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForUpdate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; init; }
    }
}

public abstract record SenatorDetails
{
    public abstract IEnumerable<SenateTerm> SenateTerms { get; }

    public sealed record ForCreate : SenatorDetails
    {
        public override IEnumerable<SenateTerm> SenateTerms => SenateTermToCreate;
        public required List<SenateTerm.ToCreate> SenateTermToCreate { get; init; }
    }
    public sealed record ForUpdate : SenatorDetails
    {
        public override IEnumerable<SenateTerm> SenateTerms => SenateTermToUpdate;
        public required List<SenateTerm.ToUpdate> SenateTermToUpdate { get; init; }
    }
}