namespace PoundPupLegacy.DomainModel;

public interface ProfessionalRoleToCreateForNewPerson : ProfessionalRole, PossiblyIdentifiable
{
    ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfNewPerson ProfessionalRoleDetailsForCreate { get; }
    public ProfessionalRoleToCreateForExistingPerson ResolvePerson(int personId);
}
public interface ProfessionalRoleToCreateForExistingPerson : ProfessionalRole, PossiblyIdentifiable
{
    ProfessionalRoleDetails.ProfessionalRoleDetailsForCreateOfExistingPerson ProfessionalRoleDetails { get; }
}
public interface ProfessionalRoleToUpdate : ProfessionalRole, CertainlyIdentifiable
{
    ProfessionalRoleDetails.ProfessionalRoleDetailsForUpdate ProfessionalRoleDetails { get; }
}
public interface ProfessionalRole : IRequest
{
}

public abstract record ProfessionalRoleDetails
{
    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }

    public sealed record ProfessionalRoleDetailsForCreateOfNewPerson : ProfessionalRoleDetails
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
    public sealed record ProfessionalRoleDetailsForCreateOfExistingPerson : ProfessionalRoleDetails
    {
        public required int? Id { get; set; }
        public required int PersonId { get; init; }
    }
    public sealed record ProfessionalRoleDetailsForUpdate : ProfessionalRoleDetails
    {
        public required int Id { get; init; }

        public required int PersonId { get; init; }
    }
}

