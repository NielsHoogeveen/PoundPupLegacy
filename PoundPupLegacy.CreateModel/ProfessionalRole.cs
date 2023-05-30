namespace PoundPupLegacy.CreateModel;

public interface ProfessionalRoleToCreateForNewPerson : ProfessionalRole, EventuallyIdentifiable
{
    ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; }
    public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId);
}
public interface ProfessionalRoleToCreateForExistingPerson : ProfessionalRole, EventuallyIdentifiable
{
    ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetailsForCreate { get; }
}
public interface ProfessionalRoleToUpdate : ProfessionalRole, ImmediatelyIdentifiable
{
    ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetailsForUpdate { get; }
}
public interface ProfessionalRole : IRequest
{
    ProfessionalRoleDetails ProfessionalRoleDetails { get; }
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

