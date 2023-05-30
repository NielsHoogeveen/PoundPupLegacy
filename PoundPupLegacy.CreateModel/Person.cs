namespace PoundPupLegacy.CreateModel;

public abstract record Person : Party
{
    private Person() { }

    public abstract LocatableDetails LocatableDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract PersonDetails PersonDetails { get; }
    public sealed record ToCreate : Person, PartyToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public override PersonDetails PersonDetails => PersonDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public required PersonDetails.PersonDetailsForCreate PersonDetailsForCreate { get; init; }
    }
    public sealed record ToUpdate : Person, PartyToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public override PersonDetails PersonDetails => PersonDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public required PersonDetails.PersonDetailsForUpdate PersonDetailsForUpdate { get; init; }
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
    public abstract T Match<T>(Func<PersonDetailsForCreate, T> create, Func<PersonDetailsForUpdate, T> update);
    public abstract void Match(Action<PersonDetailsForCreate> create, Action<PersonDetailsForUpdate> update);

    public sealed record PersonDetailsForCreate: PersonDetails
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
        public override T Match<T>(Func<PersonDetailsForCreate, T> create, Func<PersonDetailsForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<PersonDetailsForCreate> create, Action<PersonDetailsForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record PersonDetailsForUpdate : PersonDetails
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
        public override T Match<T>(Func<PersonDetailsForCreate, T> create, Func<PersonDetailsForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<PersonDetailsForCreate> create, Action<PersonDetailsForUpdate> update)
        {
            update(this);
        }
    }
}