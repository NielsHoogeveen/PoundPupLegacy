namespace PoundPupLegacy.CreateModel;

public sealed record ProfessionalRoleToCreate: ProfessionalRole, EventuallyIdentifiable
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

    public Identification Identification => IdentificationForCreate;

    public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
    public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; init; }
    public ResolvedProfessionalRoleToCreate ResolvePerson(int personId)
    {
        return new ResolvedProfessionalRoleToCreate {
            IdentificationForCreate = IdentificationForCreate,
            ProfessionalRoleDetailsForCreate = ProfessionalRoleDetailsForCreate.ResolvePerson(personId)
        };
    }
}
public sealed record ResolvedProfessionalRoleToCreate : ProfessionalRole, EventuallyIdentifiable
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }

    public Identification Identification => IdentificationForCreate;

    public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForCreate;
    public required ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; init; }
}
public sealed record ProfessionalRoleToUpdate : ProfessionalRole, ImmediatelyIdentifiable
{
    public required Identification.IdentificationForUpdate IdentificationForUpdate { get; init; }

    public Identification Identification => IdentificationForUpdate;

    public override ProfessionalRoleDetails ProfessionalRoleDetails => ProfessionalRoleDetailsForUpdate;
    public required ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; init; }
}
public abstract record ProfessionalRole : IRequest
{
    public abstract ProfessionalRoleDetails ProfessionalRoleDetails { get; }
}

public abstract record ProfessionalRoleDetails
{
    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }

    public sealed record ProfessionalRoleDetailsForCreateOfNewPerson: ProfessionalRoleDetails
    {
        public required int? Id { get; set; }

        public ProfessionalRoleDetailsForCreateOfExistingPerson ResolvePerson(int personId)
        {
            return new ProfessionalRoleDetailsForCreateOfExistingPerson {
                Id = Id,
                PersonId = personId,
                DateTimeRange = DateTimeRange,
                ProfessionId = ProfessionId
            };
        }
    }
    public sealed record ProfessionalRoleDetailsForCreateOfExistingPerson: ProfessionalRoleDetails
    {
        public required int? Id { get; set; }
        public required int PersonId { get; init; }
    }
    public sealed record ProfessionalRoleDetailsForUpdate: ProfessionalRoleDetails
    {
        public required int Id { get; init; }

        public required int PersonId { get; init; }
    }
}

