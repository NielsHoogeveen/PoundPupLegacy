namespace PoundPupLegacy.CreateModel;

public abstract record Representative : MemberOfCongress
{
    private Representative() { }
    public abstract RepresentativeDetails RepresentativeDetails { get; }
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
    public sealed record BasticProfessionalRoleToCreateForNewPerson : Representative, MemberOfCongressToCreateForNewPerson
    {
        public override RepresentativeDetails RepresentativeDetails => RepresentativeDetailsForCreate;
        public required RepresentativeDetails.RepresentativeDetailsForCreate RepresentativeDetailsForCreate { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new RepresentativeToCreateForExistingPerson {
                IdentificationForCreate = IdentificationForCreate,
                ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId),
                RepresentativeDetailsToCreate = RepresentativeDetailsForCreate,
            };
        }
    }
    public record RepresentativeToCreateForExistingPerson : Representative, MemberOfCongressToCreateForExistingPerson
    {
        public override RepresentativeDetails RepresentativeDetails => RepresentativeDetailsToCreate;
        public required RepresentativeDetails.RepresentativeDetailsForCreate RepresentativeDetailsToCreate { get; init; }
        public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

        public Identification Identification => IdentificationForCreate;

        public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
        public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
    }
    public sealed record RepresentativeToUpdate : Representative, MemberOfCongressToUpdate
    {
        public override RepresentativeDetails RepresentativeDetails => RepresentativeDetailsForUpdate;
        public required RepresentativeDetails.RepresentativeDetailsForUpdate RepresentativeDetailsForUpdate { get; init; }
        public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }

        public Identification Identification => IdentificationForUpdate;

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
        public required List<HouseTerm.HouseTermToCreate> HouseTermToCreate { get; init;}
    }
    public sealed record RepresentativeDetailsForUpdate: RepresentativeDetails
    {
        public override IEnumerable<HouseTerm> HouseTerms => HouseTermToUpdate;
        public required List<HouseTerm.HouseTermToUpdate> HouseTermToUpdate { get; init; }
    }
}