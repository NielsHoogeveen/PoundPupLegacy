namespace PoundPupLegacy.CreateModel;

public abstract record Senator : MemberOfCongress
{
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public abstract SenatorDetails RepresenetativeDetails { get; }
    public sealed record SenatorToCreate : Senator
    {
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleToCreate;
        public override SenatorDetails RepresenetativeDetails => SenatorDetailsToCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleToCreate { get; init; }
        public required SenatorDetails.SenatorDetailsToCreate SenatorDetailsToCreate { get; init; }
    }
    public sealed record SenatorToUpdate : Senator
    {
        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleToCreate;
        public override SenatorDetails RepresenetativeDetails => SenatorDetailsToCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleToCreate { get; init; }
        public required SenatorDetails.SenatorDetailsToCreate SenatorDetailsToCreate { get; init; }
    }
}
public abstract record SenatorDetails
{
    public abstract IEnumerable<HouseTerm> HouseTerms { get; }

    public sealed record SenatorDetailsToCreate : SenatorDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToCreate;
        public required List<HouseTerm.HouseTermToCreate> HouseTermToCreate { get; init; }
    }
    public sealed record SenatorDetailsToUpdate : SenatorDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToUpdate;
        public required List<HouseTerm.HouseTermToUpdate> HouseTermToUpdate { get; init; }
    }
}