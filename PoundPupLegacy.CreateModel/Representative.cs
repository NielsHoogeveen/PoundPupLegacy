namespace PoundPupLegacy.CreateModel;

public abstract record Representative : MemberOfCongress
{
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public abstract RepresentativeDetails RepresenetativeDetails { get; }
    public sealed record RepresentativeToCreate: Representative
    {
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleToCreate;
        public override RepresentativeDetails RepresenetativeDetails => RepresentativeDetailsToCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleToCreateForNewPerson ProfessionalRoleToCreate { get; init; }
        public required RepresentativeDetails.RepresentativeDetailsToCreate RepresentativeDetailsToCreate { get; init; }
    }
    public sealed record RepresentativeToUpdate : Representative
    {
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleToCreate;
        public override RepresentativeDetails RepresenetativeDetails => RepresentativeDetailsToCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleToCreateForExistingPerson ProfessionalRoleToCreate { get; init; }
        public required RepresentativeDetails.RepresentativeDetailsToCreate RepresentativeDetailsToCreate { get; init; }
    }
}
public abstract record RepresentativeDetails
{
    public abstract IEnumerable<HouseTerm> HouseTerms { get; }

    public sealed record RepresentativeDetailsToCreate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToCreate;  
        public required List<HouseTerm.HouseTermToCreate> HouseTermToCreate { get; init;}
    }
    public sealed record RepresentativeDetailsToUpdate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToUpdate;
        public required List<HouseTerm.HouseTermToUpdate> HouseTermToUpdate { get; init; }
    }
}