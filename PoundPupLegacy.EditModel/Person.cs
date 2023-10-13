namespace PoundPupLegacy.EditModel;

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

[JsonSerializable(typeof(InterPersonalRelation.From.Complete.Resolved.ToCreate), TypeInfoPropertyName = "InterPersonalRelationFromCompleteResolvedToCreate")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "InterPersonalRelationListFromCompleteResolvedToCreate")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete.Resolved.ToCreate), TypeInfoPropertyName = "InterPersonalRelationToCompleteResolvedToCreate")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "InterPersonalRelationListToCompleteResolvedToCreate")]

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

[JsonSerializable(typeof(List<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson>), TypeInfoPropertyName = "PersonPoliticalEntityRelationListCompleteResolvedToCreate")]
[JsonSerializable(typeof(PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson), TypeInfoPropertyName = "PersonPoliticalEntityRelationCompleteResolvedToCreate")]

[JsonSerializable(typeof(InterPersonalRelation.From.Complete.ToCreateForNewPerson), TypeInfoPropertyName = "InterPersonalRelationFromCompleteToCreateForNewPerson")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete.ToCreateForNewPerson), TypeInfoPropertyName = "InterPersonalRelationToCompleteToCreateForNewPerson")]

[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete.ToCreateForNewPerson>), TypeInfoPropertyName = "InterPersonalRelationListFromCompleteToCreateForNewPerson")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete.ToCreateForNewPerson>), TypeInfoPropertyName = "InterPersonalRelationListToCompleteToCreateForNewPerson")]

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

[JsonSerializable(typeof(InterPersonalRelation.From.Complete.ToCreateForNewPerson), TypeInfoPropertyName = "InterPersonalRelationFromCompleteToCreateForNewPerson")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete.ToCreateForNewPerson), TypeInfoPropertyName = "InterPersonalRelationToCompleteToCreateForNewPerson")]

[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete.ToCreateForNewPerson>), TypeInfoPropertyName = "InterPersonalRelationListFromCompleteToCreateForNewPerson")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete.ToCreateForNewPerson>), TypeInfoPropertyName = "InterPersonalRelationListToCompleteToCreateForNewPerson")]

[JsonSerializable(typeof(List<PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson>), TypeInfoPropertyName = "PersonOrganizationRelationListForPersonCompleteToCreateForNewPerson")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson), TypeInfoPropertyName = "PersonOrganizationRelationForPersonCompleteToCreateForNewPerson")]

[JsonSerializable(typeof(List<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson>), TypeInfoPropertyName = "PersonPoliticalEntityRelationListCompleteResolvedToCreate")]

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
    public abstract List<PersonPoliticalEntityRelation.Complete> PersonPoliticalEntityRelations { get; set; }
    public required List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes { get; init; }
    public abstract List<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom { get; set; }
    public abstract List<InterPersonalRelation.To.Complete> InterPersonalRelationsTo { get; set; }
    public required List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; init; }
    public abstract List<PersonOrganizationRelation.ForPerson.Complete> PersonOrganizationRelations { get; set; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }

    public sealed record ForUpdate : PersonDetails
    {
        private List<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate> existingPersonPoliticalEntityRelations = new();
        public List<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate> PartyPoliticalEntityRelationsToUpdate {
            get => existingPersonPoliticalEntityRelations;
            init {
                if (value is not null) {
                    existingPersonPoliticalEntityRelations = value;
                }
            }
        }
        private List<PersonPoliticalEntityRelation.Complete>? _personPoliticalEntityRelations = null;
        public override List<PersonPoliticalEntityRelation.Complete> PersonPoliticalEntityRelations {
            get {
                if (_personPoliticalEntityRelations is null) {
                    _personPoliticalEntityRelations = GetPersonPoliticalEntityRelations().ToList();
                }
                return _personPoliticalEntityRelations;
            }
            set {
                if (value is not null) {
                    _personPoliticalEntityRelations = value;
                }
            }
        }
        private IEnumerable<PersonPoliticalEntityRelation.Complete> GetPersonPoliticalEntityRelations()
        {
            foreach (var elem in PartyPoliticalEntityRelationsToUpdate) {
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
        public List<InterPersonalRelation.To.Complete.Resolved.ToUpdate> InterPersonalRelationsToToUpdate {
            get => existingInterPersonalRelationsTo;
            init {
                if (value is not null) {
                    existingInterPersonalRelationsTo = value;
                }
            }
        }
        private List<InterPersonalRelation.From.Complete>? _interPersonalRelationsFrom = null;
        public override List<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom {
            get {
                if (_interPersonalRelationsFrom is null) {
                    _interPersonalRelationsFrom = GetInterPersonalRelationsFrom().ToList();
                }
                return _interPersonalRelationsFrom;
            }
            set {
                if (value is not null) {
                    _interPersonalRelationsFrom = value;

                }
            }
        }
        private IEnumerable<InterPersonalRelation.From.Complete> GetInterPersonalRelationsFrom()
        {
            foreach (var elem in InterPersonalRelationsFromToUpdate) {
                yield return elem;
            }
            foreach (var elem in InterPersonalRelationsFromToCreate) {
                yield return elem;
            }
        }
        private List<InterPersonalRelation.To.Complete>? _interPersonalRelationsTo = null;
        public override List<InterPersonalRelation.To.Complete> InterPersonalRelationsTo {
            get {
                if (_interPersonalRelationsTo is null) {
                    _interPersonalRelationsTo = GetInterPersonalRelationsTo().ToList();
                }
                return _interPersonalRelationsTo;
            }
            set {
                if (value is not null) {
                    _interPersonalRelationsTo = value;
                }
            }
        }
        private IEnumerable<InterPersonalRelation.To.Complete> GetInterPersonalRelationsTo()
        {
            foreach (var elem in InterPersonalRelationsToToUpdate) {
                yield return elem;
            }
            foreach (var elem in InterPersonalRelationsToToCreate) {
                yield return elem;
            }
        }
        private List<PersonOrganizationRelation.ForPerson.Complete>? _personOrganizationRelations = null;
        public override List<PersonOrganizationRelation.ForPerson.Complete> PersonOrganizationRelations {
            get {
                if (_personOrganizationRelations is null) {
                    _personOrganizationRelations = GetPersonOrganizationRelations().ToList();
                }
                return _personOrganizationRelations;
            }
            set {
                if (value is not null) {
                    _personOrganizationRelations = value;
                }
            }
        }
        private IEnumerable<PersonOrganizationRelation.ForPerson.Complete> GetPersonOrganizationRelations()
        {
            foreach (var elem in PersonOrganizationRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in PersonOrganizationRelationsToCreate) {
                yield return elem;
            }
        }
        public List<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson> PersonPoliticalEntityRelationsToCreate { get; } = new();
        public List<InterPersonalRelation.From.Complete.Resolved.ToCreate> InterPersonalRelationsFromToCreate { get; set; } = new();
        public List<InterPersonalRelation.To.Complete.Resolved.ToCreate> InterPersonalRelationsToToCreate { get; set; } = new();
        public List<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate> PersonOrganizationRelationsToCreate { get; } = new();
        private List<InterPersonalRelation>? _interPersonalRelations = null;
        public List<InterPersonalRelation> InterPersonalRelations {
            get {
                if(_interPersonalRelations is null) {
                    _interPersonalRelations = GetInterPersonalRelations().ToList();
                }
                return _interPersonalRelations;
            }
            set {
                if(value is not null) {
                    _interPersonalRelations = value;
                }
            }
            
        }
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
        public override List<PersonPoliticalEntityRelation.Complete> PersonPoliticalEntityRelations { get; set; } = new();
        public override List<PersonOrganizationRelation.ForPerson.Complete> PersonOrganizationRelations { get; set; } = new();
        public override List<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom { get; set; } = new();
        public override List<InterPersonalRelation.To.Complete> InterPersonalRelationsTo { get; set; } = new();

    }

}