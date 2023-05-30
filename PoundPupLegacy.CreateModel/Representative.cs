namespace PoundPupLegacy.CreateModel;

public abstract record Representative : MemberOfCongress
{
    private Representative() { }
    public abstract RepresentativeDetails RepresentativeDetails { get; }
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public sealed record ToCreateForNewPerson : Representative, MemberOfCongressToCreateForNewPerson
    {
        public override RepresentativeDetails RepresentativeDetails => RepresentativeDetailsForCreate;
        public required RepresentativeDetails.RepresentativeDetailsForCreate RepresentativeDetailsForCreate { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ToCreateForExistingPerson {
                IdentificationForCreate = IdentificationForCreate,
                ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId),
                RepresentativeDetailsToCreate = RepresentativeDetailsForCreate,
            };
        }
    }
    public record ToCreateForExistingPerson : Representative, MemberOfCongressToCreateForExistingPerson
    {
        public override RepresentativeDetails RepresentativeDetails => RepresentativeDetailsToCreate;
        public required RepresentativeDetails.RepresentativeDetailsForCreate RepresentativeDetailsToCreate { get; init; }
        public required Identification.Possible IdentificationForCreate { get; init; }
        public Identification Identification => IdentificationForCreate;
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : Representative, MemberOfCongressToUpdate
    {
        public override RepresentativeDetails RepresentativeDetails => RepresentativeDetailsForUpdate;
        public required RepresentativeDetails.RepresentativeDetailsForUpdate RepresentativeDetailsForUpdate { get; init; }
        public required Identification.Certain IdentificationCertain { get; init; }
        public Identification Identification => IdentificationCertain;
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForUpdate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; init; }
    }
}

public abstract record RepresentativeDetails
{
    public abstract IEnumerable<HouseTerm> HouseTerms { get; }
    public sealed record RepresentativeDetailsForCreate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToCreate;  
        public required List<HouseTerm.ToCreate> HouseTermToCreate { get; init;}
    }
    public sealed record RepresentativeDetailsForUpdate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToUpdate;
        public required List<HouseTerm.HouseTermToUpdate> HouseTermToUpdate { get; init; }
    }
}