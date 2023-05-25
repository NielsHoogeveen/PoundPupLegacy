namespace PoundPupLegacy.CreateModel;

public interface EventuallyIdentifiableProfessionalRoleForNewPerson : ProfessionalRole, EventuallyIdentifiable
{
    public EventuallyIdentifiableProfessionalRoleForExistingPerson ResolvePerson(int personId);
}
public interface EventuallyIdentifiableProfessionalRoleForExistingPerson : ProfessionalRole, EventuallyIdentifiable
{
    int PersonId { get; }
}
public interface ImmediatelyIdentifiableProfessionalRole : ProfessionalRole, ImmediatelyIdentifiable
{
    int PersonId { get; }
}
public interface ProfessionalRole : IRequest
{
    DateTimeRange? DateTimeRange { get; }

    int ProfessionId { get; }
}

public abstract record NewProfessionalRoleBaseForNewPerson : ProfessionalRoleBase, EventuallyIdentifiableProfessionalRoleForNewPerson
{
    public required int? Id { get; set; }

    public abstract EventuallyIdentifiableProfessionalRoleForExistingPerson ResolvePerson(int personId);
}
public abstract record NewProfessionalRoleBaseForExistingPerson : ProfessionalRoleBase, EventuallyIdentifiableProfessionalRoleForExistingPerson
{
    public required int? Id { get; set; }
    public required int PersonId { get; init; }
}
public abstract record ExistingProfessionalRoleBase : ProfessionalRoleBase, ImmediatelyIdentifiableProfessionalRole
{
    public required int Id { get; init; }

    public required int PersonId { get; init; }
}

public abstract record ProfessionalRoleBase: ProfessionalRole
{
    public required DateTimeRange? DateTimeRange { get; init; }

    public required int ProfessionId { get; init; }
}

