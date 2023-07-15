namespace PoundPupLegacy.DomainModel;

public abstract record Senator : MemberOfCongress
{
    private Senator() { }
    public sealed record ToCreateForNewPerson : Senator, MemberOfCongressToCreateForNewPerson
    {
        public required SenatorDetails.ForCreate SenatorDetailsForCreate { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ToCreateForExistingPerson {
                Identification = Identification,
                ProfessionalRoleDetails = ProfessionalRoleDetailsForCreate.ResolvePerson(personId),
                SenatorDetailsToCreate = SenatorDetailsForCreate,
            };
        }
    }
    public record ToCreateForExistingPerson : Senator, MemberOfCongressToCreateForExistingPerson
    {
        public required SenatorDetails.ForCreate SenatorDetailsToCreate { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetails { get; init; }
    }
    public sealed record SenatorToUpdate : Senator, MemberOfCongressToUpdate
    {
        public required SenatorDetails.ForUpdate SenatorDetailsForUpdate { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetails { get; init; }
    }
}

public abstract record SenatorDetails
{
    public abstract IEnumerable<SenateTerm> SenateTerms { get; }

    public sealed record ForCreate : SenatorDetails
    {
        public override IEnumerable<SenateTerm.ToCreateForNewSenator> SenateTerms => SenateTermToCreate;
        public required List<SenateTerm.ToCreateForNewSenator> SenateTermToCreate { get; init; }
    }
    public sealed record ForUpdate : SenatorDetails
    {
        public override IEnumerable<SenateTerm> SenateTerms => SenateTermToUpdate;
        public required List<SenateTerm.ToUpdate> SenateTermToUpdate { get; init; }
    }
}