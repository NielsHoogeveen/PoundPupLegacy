namespace PoundPupLegacy.CreateModel;

public interface OrganizationToUpdate : Organization, PartyToUpdate
{
    OrganizationDetails.ForUpdate OrganizationDetails { get; }
}
public interface OrganizationToCreate : Organization, PartyToCreate
{
    OrganizationDetails.ForCreate OrganizationDetails { get; }
}
public interface Organization: Party
{
}

public abstract record OrganizationDetails
{
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required FuzzyDate? Established { get; init; }
    public required FuzzyDate? Terminated { get; init; }
    public required List<int> OrganizationTypeIds { get; init; }
    public abstract IEnumerable<InterOrganizationalRelation> InterOrganizationalRelations { get; }
    public abstract IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations { get; }
    public abstract IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations { get; }
    public abstract T Match<T>(Func<ForCreate, T> create, Func<ForUpdate, T> update);
    public abstract void Match(Action<ForCreate> create, Action<ForUpdate> update);

    public sealed record ForCreate: OrganizationDetails 
    {
        public override IEnumerable<InterOrganizationalRelation> InterOrganizationalRelations => GetInterOrganizationalRelations();
        public override IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations => PartyPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations => PersonOrganizationRelationsToCreate;
        public required List<InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom> InterOrganizationalRelationsFrom { get; init; }
        public required List<InterOrganizationalRelation.ToCreate.ForNewOrganizationTo> InterOrganizationalRelationsTo { get; init; }
        private IEnumerable<InterOrganizationalRelation> GetInterOrganizationalRelations()
        {
            foreach(var relation in InterOrganizationalRelationsFrom) {
                yield return relation;
            }
            foreach(var relation in InterOrganizationalRelationsTo) {
                yield return relation;
            }
        }
        public required List<PartyPoliticalEntityRelation.ToCreate.ForNewParty> PartyPoliticalEntityRelationsToCreate { get; init; }

        public required List<PersonOrganizationRelation.ToCreate.ForNewOrganization> PersonOrganizationRelationsToCreate { get; init; }
        public override T Match<T>(Func<ForCreate, T> create, Func<ForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<ForCreate> create, Action<ForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record ForUpdate : OrganizationDetails
    {
        public override IEnumerable<InterOrganizationalRelation> InterOrganizationalRelations => InterOrganizationalRelationsToCreate;
        public override IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations => PartyPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations => PersonOrganizationRelationsToCreate;
        public required List<InterOrganizationalRelation.ToCreate.ForExistingParticipants> InterOrganizationalRelationsToCreate { get; init; }
        public required List<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> PartyPoliticalEntityRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.ToCreate.ForExistingParticipants> PersonOrganizationRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.ToUpdate> PersonOrganizationRelationsToUpdate { get; init; }
        public required List<PartyPoliticalEntityRelation.ToUpdate> PartyPoliticalEntityRelationToUpdates { get; init; }
        public override T Match<T>(Func<ForCreate, T> create, Func<ForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<ForCreate> create, Action<ForUpdate> update)
        {
            update(this);
        }
    }
}
