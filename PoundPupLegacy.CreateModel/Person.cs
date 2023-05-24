namespace PoundPupLegacy.CreateModel;

public sealed record NewPerson : NewNameableBase, EventuallyIdentifiablePerson
{
    public required DateTime? DateOfBirth { get; init; }
    public required DateTime? DateOfDeath { get; init; }
    public required int? FileIdPortrait { get; init; }
    public required string? FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string? FullName { get; init; }
    public required string? Suffix { get; init; }
    public required int? GovtrackId { get; init; }
    public required string? Bioguide { get; init; }
    public required List<EventuallyIdentifiableProfessionalRole> ProfessionalRoles { get; init; }
    public required List<NewPersonOrganizationRelation> PersonOrganizationRelations { get; init; }
}
public sealed record ExistingPerson : ExistingNameableBase, ImmediatelyIdentifiablePerson
{
    public required DateTime? DateOfBirth { get; init; }
    public required DateTime? DateOfDeath { get; init; }
    public required int? FileIdPortrait { get; init; }
    public required string? FirstName { get; init; }
    public required string? MiddleName { get; init; }
    public required string? LastName { get; init; }
    public required string? FullName { get; init; }
    public required string? Suffix { get; init; }
    public required int? GovtrackId { get; init; }
    public required string? Bioguide { get; init; }
    public required List<EventuallyIdentifiableProfessionalRole> ProfessionalRoles { get; init; }
    public required List<NewPersonOrganizationRelation> PersonOrganizationRelations { get; init; }
}
public interface ImmediatelyIdentifiablePerson : Person, ImmediatelyIdentifiableParty
{
}
public interface EventuallyIdentifiablePerson : Person, EventuallyIdentifiableParty
{
}
public interface Person : Party
{
    DateTime? DateOfBirth { get; }
    DateTime? DateOfDeath { get; }
    int? FileIdPortrait { get; }
    string? FirstName { get; }
    string? MiddleName { get; }
    string? LastName { get; }
    string? FullName { get; }
    string? Suffix { get; }
    int? GovtrackId { get; }
    string? Bioguide { get; }
    List<EventuallyIdentifiableProfessionalRole> ProfessionalRoles { get; }
    List<NewPersonOrganizationRelation> PersonOrganizationRelations { get; }
}
