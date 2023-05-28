namespace PoundPupLegacy.CreateModel;

public abstract record Person : Locatable
{
    private Person() { }

    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract OrganizationDetails PersonDetails { get; }
    public abstract T Match<T>(Func<PersonToCreate, T> create, Func<PersonToUpdate, T> update);
    public abstract void Match(Action<PersonToCreate> create, Action<PersonToUpdate> update);

    public sealed record PersonToCreate : Person, LocatableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public override OrganizationDetails PersonDetails => OrganizationDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForCreate OrganizationDetailsForCreate { get; init; }
        public override T Match<T>(Func<PersonToCreate, T> create, Func<PersonToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<PersonToCreate> create, Action<PersonToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record PersonToUpdate : Person, LocatableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public override OrganizationDetails PersonDetails => OrganizationDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public required OrganizationDetails.OrganizationDetailsForUpdate OrganizationDetailsForUpdate { get; init; }
        public override T Match<T>(Func<PersonToCreate, T> create, Func<PersonToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<PersonToCreate> create, Action<PersonToUpdate> update)
        {
            update(this);
        }
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
        public required List<InterPersonalRelation.InterPersonalRelationToCreateForNewPersonFrom> InterPersonalRelationsToCreateFrom { get; init; }
        public required List<InterPersonalRelation.InterPersonalRelationToCreateForNewPersonTo> InterPersonalRelationsToCreateTo { get; init; }
        private IEnumerable<InterPersonalRelation> GetInterPersonalRelations()
        {
            foreach(var relation in InterPersonalRelationsToCreateFrom) {
                  yield return relation;
            }
            foreach (var relation in InterPersonalRelationsToCreateTo) {
                yield return relation;
            }
        }
        public required List<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForNewParty> PartyPoliticalEntityRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.PersonOrganizationRelationToCreateForNewPerson> PersonOrganizationRelationToCreate { get; init; }
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
        public required List<InterPersonalRelation.InterPersonalRelationToCreateForExistingParticipants> InterPersonalRelationsToCreate { get; init; }
        public required List<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToCreateForExistingParty> PartyPoliticalEntityRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.PersonOrganizationRelationToCreateForExistingParticipants> PersonOrganizationRelationToCreate { get; init; }
        public required List<InterPersonalRelation.InterPersonalRelationToUpdate> InterPersonalRelationToUpdates { get; init; }
        public required List<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToUpdate> PartyPoliticalEntityRelationToUpdates { get; init; }
        public required List<PersonOrganizationRelation.PersonOrganizationRelationToUpdate> PersonOrganizationRelationToUpdates { get; init; }
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