namespace PoundPupLegacy.CreateModel;

public sealed record NewPerson : NewPartyBase, EventuallyIdentifiablePerson
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
    public required List<EventuallyIdentifiableProfessionalRoleForNewPerson> ProfessionalRoles { get; init; }
    public required List<EventuallyIdentifiablePersonOrganizationRelationForNewPerson> PersonOrganizationRelations { get; init; }
    public required List<EventuallyIdentifiableInterPersonalRelationForNewPersonTo> InterPersonalRelationsToAddTo { get; init; }
    public required List<EventuallyIdentifiableInterPersonalRelationForNewPersonFrom> InterPersonalRelationsToAddFrom { get; init; }
}
public sealed record ExistingPerson : ExistingPartyBase, ImmediatelyIdentifiablePerson
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
    public required List<EventuallyIdentifiableProfessionalRoleForExistingPerson> ProfessionalRolesToAdd { get; init; }
    public required List<ImmediatelyIdentifiablePersonOrganizationRelation> PersonOrganizationRelationsToUpdate { get; init; }
    public required List<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> PersonOrganizationRelationsToAdd { get; init; }
    public required List<ImmediatelyIdentifiableInterPersonalRelation> InterPersonalRelationsToUpdate { get; init; }
    public required List<EventuallyIdentifiableInterPersonalRelationForExistingParticipants> InterPersonalRelationsToAdd { get; init; }

}
public interface ImmediatelyIdentifiablePerson : Person, ImmediatelyIdentifiableParty
{
    List<EventuallyIdentifiableProfessionalRoleForExistingPerson> ProfessionalRolesToAdd { get; }
    List<ImmediatelyIdentifiablePersonOrganizationRelation> PersonOrganizationRelationsToUpdate { get; }
    List<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> PersonOrganizationRelationsToAdd { get; }
    List<ImmediatelyIdentifiableInterPersonalRelation> InterPersonalRelationsToUpdate { get; }
    List<EventuallyIdentifiableInterPersonalRelationForExistingParticipants> InterPersonalRelationsToAdd { get; }

}
public interface EventuallyIdentifiablePerson : Person, EventuallyIdentifiableParty
{
    List<EventuallyIdentifiableProfessionalRoleForNewPerson> ProfessionalRoles { get; }
    List<EventuallyIdentifiablePersonOrganizationRelationForNewPerson> PersonOrganizationRelations { get; }
    List<EventuallyIdentifiableInterPersonalRelationForNewPersonFrom> InterPersonalRelationsToAddFrom { get; }
    List<EventuallyIdentifiableInterPersonalRelationForNewPersonTo> InterPersonalRelationsToAddTo { get; }
    
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
    
}
