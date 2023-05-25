namespace PoundPupLegacy.CreateModel;
public interface ImmediatelyIdentifiableOrganization : Organization, ImmediatelyIdentifiableParty
{
    List<int> OrganizationTypeIdsToAdd { get; }
    List<int> OrganizationTypeIdsToRemove { get; }
    List<ImmediatelyIdentifiablePersonOrganizationRelation> PersonOrganizationRelationsToUpdate { get; }
    List<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> PersonOrganizationRelationsToAdd { get; }
    List<ImmediatelyIdentifiableInterOrganizationalRelation> InterOrganizationalRelationsToUpdate { get; }
    List<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants> InterOrganizationalRelationsToAdd { get; }
}
public interface EventuallyIdentifiableOrganization : Organization, EventuallyIdentifiableParty
{
    List<int> OrganizationTypeIds { get; }
    List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization> PersonOrganizationRelations { get; }
    List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo> InterOrganizationalRelationsToAddFrom { get; }
    List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo> InterOrganizationalRelationsToAddTo { get; }
}
public interface Organization : Party
{
    string? WebsiteUrl { get; }
    string? EmailAddress { get; }
    FuzzyDate? Established { get; }
    FuzzyDate? Terminated { get; }
    
}

public abstract record NewOrganizationBase: NewPartyBase, EventuallyIdentifiableOrganization
{
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required FuzzyDate? Established { get; init; }
    public required FuzzyDate? Terminated { get; init; }
    public required List<int> OrganizationTypeIds { get; init; }
    public required List<EventuallyIdentifiablePersonOrganizationRelationForNewOrganization> PersonOrganizationRelations { get; init; }
    public required List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo> InterOrganizationalRelationsToAddFrom { get; init; }
    public required List<EventuallyIdentifiableInterOrganizationalRelationForNewOrganizationTo> InterOrganizationalRelationsToAddTo { get; init; }

}
public abstract record ExistingOrganizationBase : ExistingPartyBase, ImmediatelyIdentifiableOrganization
{
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required FuzzyDate? Established { get; init; }
    public required FuzzyDate? Terminated { get; init; }
    public required List<int> OrganizationTypeIdsToAdd { get; init; }
    public required List<int> OrganizationTypeIdsToRemove { get; init; }
    public required List<ImmediatelyIdentifiablePersonOrganizationRelation> PersonOrganizationRelationsToUpdate { get; init; }
    public required List<EventuallyIdentifiablePersonOrganizationRelationForExistingParticipants> PersonOrganizationRelationsToAdd { get; init; }
    public required List<ImmediatelyIdentifiableInterOrganizationalRelation> InterOrganizationalRelationsToUpdate { get; init; }
    public required List<EventuallyIdentifiableInterOrganizationalRelationForExistingParticipants> InterOrganizationalRelationsToAdd { get; init; }

}