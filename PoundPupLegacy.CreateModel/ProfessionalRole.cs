namespace PoundPupLegacy.CreateModel;

public interface ProfessionalRoleToCreate: ProfessionalRole
{
    ProfessionalRoleDetails.ProfessionalRoleToCreateForNewPerson ProfessionalRoleToCreate { get; }
}
public interface ProfessionalRoleToUpdate : ProfessionalRole
{
    ProfessionalRoleDetails.ProfessionalRoleToCreateForExistingPerson ProfessionalRoleToUpdate { get; }
}
public interface ProfessionalRole : IRequest
{
    ProfessionalRoleDetails ProfessionalRoleDetails { get; }
}

public abstract record ProfessionalRoleDetails
{
    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }

    public sealed record ProfessionalRoleToCreateForNewPerson: ProfessionalRoleDetails
    {
        public required int? Id { get; set; }

        public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId)
        {
            return new ProfessionalRoleToCreateForExistingPerson {
                Id = Id,
                PersonId = personId,
                DateTimeRange = DateTimeRange,
                ProfessionId = ProfessionId
            };
        }
    }
    public sealed record ProfessionalRoleToCreateForExistingPerson: ProfessionalRoleDetails
    {
        public required int? Id { get; set; }
        public required int PersonId { get; init; }
    }
    public sealed record ExistingProfessionalRoleBase: ProfessionalRoleDetails
    {
        public required int Id { get; init; }

        public required int PersonId { get; init; }
    }
}

