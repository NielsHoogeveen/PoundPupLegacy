﻿namespace PoundPupLegacy.CreateModel;

public abstract record Person : Party
{
    private Person() { }

    public sealed record ToCreate : Person, PartyToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; init; }
        public required PersonDetails.ForCreate PersonDetails { get; init; }
    }
    public sealed record ToUpdate : Person, PartyToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; init; }
        public required PersonDetails.ForUpdate PersonDetails { get; init; }
    }
}
public abstract record PersonDetails
{
    private PersonDetails() { }
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
    public abstract IEnumerable<InterPersonalRelation> InterPersonalRelations { get; }
    public abstract IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations { get; }
    public abstract IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations { get; }

    public sealed record ForCreate: PersonDetails
    {
        public override IEnumerable<InterPersonalRelation> InterPersonalRelations => GetInterPersonalRelations();
        public override IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations => PartyPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations => PersonOrganizationRelationToCreate;
        public required List<InterPersonalRelation.ToCreateForNewPersonFrom> InterPersonalRelationsToCreateFrom { get; init; }
        public required List<InterPersonalRelation.ToCreateForNewPersonTo> InterPersonalRelationsToCreateTo { get; init; }
        private IEnumerable<InterPersonalRelation> GetInterPersonalRelations()
        {
            foreach(var relation in InterPersonalRelationsToCreateFrom) {
                  yield return relation;
            }
            foreach (var relation in InterPersonalRelationsToCreateTo) {
                yield return relation;
            }
        }
        public required List<PartyPoliticalEntityRelation.ToCreateForNewParty> PartyPoliticalEntityRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.ToCreateForNewPerson> PersonOrganizationRelationToCreate { get; init; }

        public required List<ProfessionalRoleToCreateForNewPerson> ProfessionalRolesToCreate { get; init; }
    }
    public sealed record ForUpdate : PersonDetails
    {
        public override IEnumerable<InterPersonalRelation> InterPersonalRelations => InterPersonalRelationsToCreate;
        public override IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations => PartyPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations => PersonOrganizationRelationToCreate;
        public required List<InterPersonalRelation.ToCreateForExistingParticipants> InterPersonalRelationsToCreate { get; init; }
        public required List<PartyPoliticalEntityRelation.ToCreateForExistingParty> PartyPoliticalEntityRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.ToCreateForExistingParticipants> PersonOrganizationRelationToCreate { get; init; }
        public required List<InterPersonalRelation.InterPersonalRelationToUpdate> InterPersonalRelationToUpdates { get; init; }
        public required List<PartyPoliticalEntityRelation.ToUpdate> PartyPoliticalEntityRelationToUpdates { get; init; }
        public required List<PersonOrganizationRelation.ToUpdate> PersonOrganizationRelationToUpdates { get; init; }
        public required List<ProfessionalRoleToCreateForNewPerson> ProfessionalRolesToCreate { get; init; }
    }
}