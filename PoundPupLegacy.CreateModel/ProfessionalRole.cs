namespace PoundPupLegacy.CreateModel;

public interface EventuallyIdentifiableProfessionalRole : ProfessionalRole, EventuallyIdentifiable
{
}
public interface ProfessionalRole : IRequest
{
    int? PersonId { get; set; }
    DateTimeRange? DateTimeRange { get; }

    int ProfessionId { get; }
}
public abstract record ProfessionalRoleBase: ProfessionalRole
{
    public required int? PersonId { get; set; }
    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }
}

