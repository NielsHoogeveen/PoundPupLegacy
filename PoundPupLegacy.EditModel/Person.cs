﻿namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Person.ToUpdate), TypeInfoPropertyName = "PersonToUpdate")]

[JsonSerializable(typeof(PersonDetails.ForUpdate), TypeInfoPropertyName = "PersonDetailsForUpdate")]

[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]

[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationIterableFromComplete")]
[JsonSerializable(typeof(InterPersonalRelation.From.Complete), TypeInfoPropertyName = "InterPersonalRelationFromComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationListFromComplete")]

[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationEnumerableToComplete")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete), TypeInfoPropertyName = "InterPersonalRelationToComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationListToComplete")]

[JsonSerializable(typeof(InterPersonalRelation.From.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterPersonalRelationFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterPersonalRelationListFromCompleteResolvedToUpdate")]

[JsonSerializable(typeof(InterPersonalRelation.To.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterPersonalRelationToCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterPersonalRelationListToCompleteResolvedToUpdate")]

[JsonSerializable(typeof(IEnumerable<PersonOrganizationRelation.ForPerson.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationEnumerableForPersonComplete")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForPerson.Complete), TypeInfoPropertyName = "PersonOrganizationRelationForPersonComplete")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForPerson.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationListForPersonComplete")]

[JsonSerializable(typeof(PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate), TypeInfoPropertyName = "PersonOrganizationRelationForPersonCompleteResolvedToCreate")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "PersonOrganizationRelationForPersonCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "PersonOrganizationRelationListForPersonCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "PersonOrganizationRelationListForPersonCompleteResolvedToCreate")]

[JsonSerializable(typeof(IEnumerable<PersonPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "PersonPoliticalEntityEnumerableRelationComplete")]
[JsonSerializable(typeof(PersonPoliticalEntityRelation.Complete), TypeInfoPropertyName = "PersonPoliticalEntityRelationComplete")]
[JsonSerializable(typeof(List<PersonPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "PersonPoliticalEntityRelationListComplete")]

[JsonSerializable(typeof(PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "PersonPoliticalEntityRelationCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "PersonPoliticalEntityRelationListCompleteResolvedToUpdate")]

[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]

[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]

[JsonSerializable(typeof(TenantNodeDetails.ForUpdate), TypeInfoPropertyName = "TenantNodeDetailsForUpdate")]
[JsonSerializable(typeof(TenantNodeDetails.ForCreate), TypeInfoPropertyName = "TenantNodeDetailsForCreate")]

[JsonSerializable(typeof(TenantNodeDetails.ForUpdate), TypeInfoPropertyName = "TenantNodeDetailsForUpdate")]

[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(Tags.ToUpdate), TypeInfoPropertyName = "TagsToUpdate")]
[JsonSerializable(typeof(List<Tags.ToUpdate>), TypeInfoPropertyName = "TagsListToUpdate")]

public partial class PersonToUpdateJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(PersonDetails.ForCreate), TypeInfoPropertyName = "PersonDetailsForCreate")]

[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]

[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationIterableFromComplete")]
[JsonSerializable(typeof(Person.ToCreate), TypeInfoPropertyName = "PersonToCreate")]
[JsonSerializable(typeof(InterPersonalRelation.From.Complete), TypeInfoPropertyName = "InterPersonalRelationFromComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationListFromComplete")]

[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationEnumerableToComplete")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete), TypeInfoPropertyName = "InterPersonalRelationToComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationListToComplete")]

[JsonSerializable(typeof(IEnumerable<PersonOrganizationRelation.ForPerson.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationEnumerableForPersonComplete")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForPerson.Complete), TypeInfoPropertyName = "PersonOrganizationRelationForPersonComplete")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForPerson.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationListForPersonComplete")]

[JsonSerializable(typeof(IEnumerable<PersonPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "PersonPoliticalEntityEnumerableRelationComplete")]
[JsonSerializable(typeof(PersonPoliticalEntityRelation.Complete), TypeInfoPropertyName = "PersonPoliticalEntityRelationComplete")]
[JsonSerializable(typeof(List<PersonPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "PersonPoliticalEntityRelationListComplete")]

[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]

[JsonSerializable(typeof(TenantNodeDetails.ForCreate), TypeInfoPropertyName = "TenantNodeDetailsForCreate")]

[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(List<Tags.ToCreate>), TypeInfoPropertyName = "TagsListToCreate")]
public partial class PersonToCreateJsonContext : JsonSerializerContext { }

public abstract record Person: Locatable, ResolvedNode, Node<Person.ToUpdate, Person.ToCreate>, Resolver<Person.ToUpdate, Person.ToCreate, Unit>
{
    private Person() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;

    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }

    public abstract PersonDetails PersonDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : Person, ExistingNode, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override PersonDetails PersonDetails => PersonDetailsForUpdate;
        public required PersonDetails.ForUpdate PersonDetailsForUpdate { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate : Person, NewLocatable, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }
        public override PersonDetails PersonDetails => PersonDetailsForCreate;
        public required PersonDetails.ForCreate PersonDetailsForCreate { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            newItem(this);
        }
    }
}

public abstract record PersonDetails
{
    public required string Name { get; init; }
    public abstract IEnumerable<PersonPoliticalEntityRelation.Complete> PersonPoliticalEntityRelations { get; }
    public required List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes { get; init; }
    public abstract IEnumerable<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom { get; }
    public abstract IEnumerable<InterPersonalRelation.To.Complete> InterPersonalRelationsTo { get; }
    public required List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; init; }
    public abstract IEnumerable<PersonOrganizationRelation.ForPerson.Complete> PersonOrganizationRelations { get; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }

    public sealed record ForUpdate: PersonDetails
    {
        private List<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate> existingPersonPoliticalEntityRelations = new();
        public List<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate> PersonPoliticalEntityRelationsToUpdate {
            get => existingPersonPoliticalEntityRelations;
            init {
                if (value is not null) {
                    existingPersonPoliticalEntityRelations = value;
                }
            }
        }
        public override IEnumerable<PersonPoliticalEntityRelation.Complete> PersonPoliticalEntityRelations => GetPersonPoliticalEntityRelations();
        private IEnumerable<PersonPoliticalEntityRelation.Complete> GetPersonPoliticalEntityRelations()
        {
            foreach (var elem in PersonPoliticalEntityRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in PersonPoliticalEntityRelationsToCreate) {
                yield return elem;
            }
        }
        private List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate> existingPersonOrganizationRelations = new();
        public List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate> PersonOrganizationRelationsToUpdate {
            get => existingPersonOrganizationRelations;
            init {
                if (value is not null) {
                    existingPersonOrganizationRelations = value;
                }
            }
        }
        private List<InterPersonalRelation.From.Complete.Resolved.ToUpdate> existingInterPersonalRelationsFrom = new();
        public List<InterPersonalRelation.From.Complete.Resolved.ToUpdate> InterPersonalRelationsFromToUpdate {
            get => existingInterPersonalRelationsFrom;
            init {
                if (value is not null) {
                    existingInterPersonalRelationsFrom = value;
                }
            }
        }
        private List<InterPersonalRelation.To.Complete.Resolved.ToUpdate> existingInterPersonalRelationsTo = new();
        public List<InterPersonalRelation.To.Complete.Resolved.ToUpdate> InterPersonalRelationsToUpdate {
            get => existingInterPersonalRelationsTo;
            init {
                if (value is not null) {
                    existingInterPersonalRelationsTo = value;
                }
            }
        }
        public override IEnumerable<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom => GetInterPersonalRelationsFrom();
        private IEnumerable<InterPersonalRelation.From.Complete> GetInterPersonalRelationsFrom()
        {
            foreach (var elem in InterPersonalRelationsFromToUpdate) {
                yield return elem;
            }
            foreach (var elem in InterPersonalRelationsFromToCreate) {
                yield return elem;
            }
        }
        public override IEnumerable<InterPersonalRelation.To.Complete> InterPersonalRelationsTo => GetInterPersonalRelationsTo();
        private IEnumerable<InterPersonalRelation.To.Complete> GetInterPersonalRelationsTo()
        {
            foreach (var elem in InterPersonalRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in InterPersonalRelationsToToCreate) {
                yield return elem;
            }
        }
        public override IEnumerable<PersonOrganizationRelation.ForPerson.Complete> PersonOrganizationRelations => GetPersonOrganizationRelations();
        private IEnumerable<PersonOrganizationRelation.ForPerson.Complete> GetPersonOrganizationRelations()
        {
            foreach (var elem in PersonOrganizationRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in PersonOrganizationRelationsToCreate) {
                yield return elem;
            }
        }
        public List<PersonPoliticalEntityRelation.Complete.Resolved.ToCreate> PersonPoliticalEntityRelationsToCreate { get; } = new();
        public List<InterPersonalRelation.From.Complete> InterPersonalRelationsFromToCreate { get; set; } = new();
        public List<InterPersonalRelation.To.Complete> InterPersonalRelationsToToCreate { get; set; } = new();
        public List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate> PersonOrganizationRelationsToCreate { get; } = new();
        public IEnumerable<InterPersonalRelation> InterPersonalRelations => GetInterPersonalRelations();
        private IEnumerable<InterPersonalRelation> GetInterPersonalRelations()
        {
            foreach (var elem in InterPersonalRelationsFrom) {
                yield return elem;
            }
            foreach (var elem in InterPersonalRelationsTo) {
                yield return elem;
            }
        }

    }

    public sealed record ForCreate: PersonDetails
    {
        public override IEnumerable<PersonPoliticalEntityRelation.Complete> PersonPoliticalEntityRelations => PersonPoliticalEntityRelationsToCreate;
        public override IEnumerable<PersonOrganizationRelation.ForPerson.Complete> PersonOrganizationRelations => PersonOrganizationRelationsToCreate;
        public override IEnumerable<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom => InterPersonalRelationsFromToCreate;
        public override IEnumerable<InterPersonalRelation.To.Complete> InterPersonalRelationsTo => InterPersonalRelationsToToCreate;
        public List<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson> PersonPoliticalEntityRelationsToCreate { get; } = new();
        public List<InterPersonalRelation.From.Complete.ToCreateForNewPerson> InterPersonalRelationsFromToCreate { get; set; } = new();
        public List<InterPersonalRelation.To.Complete.ToCreateForNewPerson> InterPersonalRelationsToToCreate { get; set; } = new();
        public List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreateForNewPerson> PersonOrganizationRelationsToCreate { get; } = new();

    }

}