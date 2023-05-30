﻿namespace PoundPupLegacy.CreateModel;

public interface OrganizationToUpdate : Organization, PartyToUpdate
{
    OrganizationDetails.OrganizationDetailsForUpdate OrganizationDetails { get; }
}
public interface OrganizationToCreate : Organization, PartyToCreate
{
    OrganizationDetails.OrganizationDetailsForCreate OrganizationDetails { get; }
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
    public abstract T Match<T>(Func<OrganizationDetailsForCreate, T> create, Func<OrganizationDetailsForUpdate, T> update);
    public abstract void Match(Action<OrganizationDetailsForCreate> create, Action<OrganizationDetailsForUpdate> update);

    public sealed record OrganizationDetailsForCreate: OrganizationDetails 
    {
        public override IEnumerable<InterOrganizationalRelation> InterOrganizationalRelations => GetInterOrganizationalRelations();
        public override IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations => PartyPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations => PersonOrganizationRelationsToCreate;
        public required List<InterOrganizationalRelation.ToCreateForNewOrganizationFrom> InterOrganizationalRelationsFrom { get; init; }
        public required List<InterOrganizationalRelation.ToCreateForNewOrganizationTo> InterOrganizationalRelationsTo { get; init; }
        private IEnumerable<InterOrganizationalRelation> GetInterOrganizationalRelations()
        {
            foreach(var relation in InterOrganizationalRelationsFrom) {
                yield return relation;
            }
            foreach(var relation in InterOrganizationalRelationsTo) {
                yield return relation;
            }
        }
        public required List<PartyPoliticalEntityRelation.ToCreateForNewParty> PartyPoliticalEntityRelationsToCreate { get; init; }

        public required List<PersonOrganizationRelation.ToCreateForNewOrganization> PersonOrganizationRelationsToCreate { get; init; }
        public override T Match<T>(Func<OrganizationDetailsForCreate, T> create, Func<OrganizationDetailsForUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<OrganizationDetailsForCreate> create, Action<OrganizationDetailsForUpdate> update)
        {
            create(this);
        }
    }
    public sealed record OrganizationDetailsForUpdate : OrganizationDetails
    {
        public override IEnumerable<InterOrganizationalRelation> InterOrganizationalRelations => InterOrganizationalRelationsToCreate;
        public override IEnumerable<PartyPoliticalEntityRelation> PartyPoliticalEntityRelations => PartyPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation> PersonOrganizationRelations => PersonOrganizationRelationsToCreate;
        public required List<InterOrganizationalRelation.ToCreateForExistingParticipants> InterOrganizationalRelationsToCreate { get; init; }
        public required List<PartyPoliticalEntityRelation.ToCreateForExistingParty> PartyPoliticalEntityRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.ToCreateForExistingParticipants> PersonOrganizationRelationsToCreate { get; init; }
        public required List<PersonOrganizationRelation.ToUpdate> PersonOrganizationRelationsToUpdate { get; init; }
        public required List<PartyPoliticalEntityRelation.ToUpdate> PartyPoliticalEntityRelationToUpdates { get; init; }
        public required List<PersonOrganizationRelation.ToUpdate> PersonOrganizationRelationToUpdates { get; init; }
        public override T Match<T>(Func<OrganizationDetailsForCreate, T> create, Func<OrganizationDetailsForUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<OrganizationDetailsForCreate> create, Action<OrganizationDetailsForUpdate> update)
        {
            update(this);
        }
    }
}
