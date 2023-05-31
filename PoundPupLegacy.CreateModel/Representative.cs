namespace PoundPupLegacy.CreateModel;

public abstract record Representative : MemberOfCongress
{
    private Representative() { }
    public sealed record ToCreateForNewPerson : Representative, MemberOfCongressToCreateForNewPerson
    {
        public required RepresentativeDetails.ForCreate RepresentativeDetailsForCreate { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ToCreateForExistingPerson {
                Identification = Identification,
                ProfessionalRoleDetails = ProfessionalRoleDetailsForCreate.ResolvePerson(personId),
                RepresentativeDetailsToCreate = RepresentativeDetailsForCreate,
            };
        }
    }
    public record ToCreateForExistingPerson : Representative, MemberOfCongressToCreateForExistingPerson
    {
        public required RepresentativeDetails.ForCreate RepresentativeDetailsToCreate { get; init; }
        public required Identification.Possible Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetails { get; init; }
    }
    public sealed record ToUpdate : Representative, MemberOfCongressToUpdate
    {
        public required RepresentativeDetails.ForUpdate RepresentativeDetailsForUpdate { get; init; }
        public required Identification.Certain Identification { get; init; }
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetails { get; init; }
    }
}

public abstract record RepresentativeDetails
{
    public abstract IEnumerable<HouseTerm> HouseTerms { get; }
    public sealed record ForCreate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm.ToCreateForNewRepresenatative> HouseTerms => HouseTermToCreate;  
        public required List<HouseTerm.ToCreateForNewRepresenatative> HouseTermToCreate { get; init;}
    }
    public sealed record ForUpdate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToUpdate;
        public required List<HouseTerm.HouseTermToUpdate> HouseTermToUpdate { get; init; }
    }
}