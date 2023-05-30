namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Person.ToUpdate))]
[JsonSerializable(typeof(InterPersonalRelation.From.Complete), TypeInfoPropertyName = "InterPersonalRelationFromComplete")]
[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationIterableFromComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationListFromComplete")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete), TypeInfoPropertyName = "InterPersonalRelationToComplete")]
[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationEnumerableToComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationListToComplete")]
[JsonSerializable(typeof(InterPersonalRelation.From.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterPersonalRelationFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterPersonalRelationListFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterPersonalRelationToCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterPersonalRelationListToCompleteResolvedToUpdate")]
public partial class ExistingPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Person.ToCreate), TypeInfoPropertyName = "PersonToCreate")]
[JsonSerializable(typeof(InterPersonalRelation.From.Complete), TypeInfoPropertyName = "InterPersonalRelationFromComplete")]
[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationIterableFromComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.From.Complete>), TypeInfoPropertyName = "InterPersonalRelationListFromComplete")]
[JsonSerializable(typeof(InterPersonalRelation.To.Complete), TypeInfoPropertyName = "InterPersonalRelationToComplete")]
[JsonSerializable(typeof(IEnumerable<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationEnumerableToComplete")]
[JsonSerializable(typeof(List<InterPersonalRelation.To.Complete>), TypeInfoPropertyName = "InterPersonalRelationListToComplete")]
public partial class NewPersonJsonContext : JsonSerializerContext { }

public abstract record Person : Locatable, ResolvedNode
{
    private Person() { }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations { get; }
    public required List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes { get; init; }
    public abstract IEnumerable<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom { get; }
    public abstract IEnumerable<InterPersonalRelation.To.Complete> InterPersonalRelationsTo { get; }
    public required List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; init; }
    public abstract IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations { get; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }
    public sealed record ToUpdate : Person, ExistingNode, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        private List<ExistingPersonPoliticalEntityRelation> existingPersonPoliticalEntityRelations = new();
        public List<ExistingPersonPoliticalEntityRelation> ExistingPersonPoliticalEntityRelations {
            get => existingPersonPoliticalEntityRelations;
            init {
                if (value is not null) {
                    existingPersonPoliticalEntityRelations = value;
                }
            }
        }
        public override IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations => GetPersonPoliticalEntityRelations();
        private IEnumerable<CompletedPersonPoliticalEntityRelation> GetPersonPoliticalEntityRelations()
        {
            foreach (var elem in ExistingPersonPoliticalEntityRelations) {
                yield return elem;
            }
            foreach (var elem in NewPersonPoliticalEntityRelations) {
                yield return elem;
            }
        }
        private List<ExistingPersonOrganizationRelationForPerson> existingPersonOrganizationRelations = new();
        public List<ExistingPersonOrganizationRelationForPerson> ExistingPersonOrganizationRelations {
            get => existingPersonOrganizationRelations;
            init {
                if (value is not null) {
                    existingPersonOrganizationRelations = value;
                }
            }
        }
        private List<InterPersonalRelation.From.Complete.Resolved.ToUpdate> existingInterPersonalRelationsFrom = new();
        public List<InterPersonalRelation.From.Complete.Resolved.ToUpdate> ExistingInterPersonalRelationsFrom {
            get => existingInterPersonalRelationsFrom;
            init {
                if (value is not null) {
                    existingInterPersonalRelationsFrom = value;
                }
            }
        }
        private List<InterPersonalRelation.To.Complete.Resolved.ToUpdate> existingInterPersonalRelationsTo = new();
        public List<InterPersonalRelation.To.Complete.Resolved.ToUpdate> ExistingInterPersonalRelationsTo {
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
            foreach (var elem in ExistingInterPersonalRelationsFrom) {
                yield return elem;
            }
            foreach (var elem in NewInterPersonalRelationsFrom) {
                yield return elem;
            }
        }
        public override IEnumerable<InterPersonalRelation.To.Complete> InterPersonalRelationsTo => GetInterPersonalRelationsTo();
        private IEnumerable<InterPersonalRelation.To.Complete> GetInterPersonalRelationsTo()
        {
            foreach (var elem in ExistingInterPersonalRelationsTo) {
                yield return elem;
            }
            foreach (var elem in NewInterPersonalRelationsTo) {
                yield return elem;
            }
        }
        public override IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations => GetPersonOrganizationRelations();
        private IEnumerable<CompletedPersonOrganizationRelationForPerson> GetPersonOrganizationRelations()
        {
            foreach (var elem in ExistingPersonOrganizationRelations) {
                yield return elem;
            }
            foreach (var elem in NewPersonOrganizationRelations) {
                yield return elem;
            }
        }
        public List<CompletedPersonPoliticalEntityRelation> NewPersonPoliticalEntityRelations { get; } = new();
        public List<InterPersonalRelation.From.Complete> NewInterPersonalRelationsFrom { get; set; } = new();
        public List<InterPersonalRelation.To.Complete> NewInterPersonalRelationsTo { get; set; } = new();
        public List<CompletedNewPersonOrganizationRelationForPerson> NewPersonOrganizationRelations { get; } = new();
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
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations => NewPersonPoliticalEntityRelations;
        public override IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations => NewPersonOrganizationRelations;
        public override IEnumerable<InterPersonalRelation.From.Complete> InterPersonalRelationsFrom => NewInterPersonalRelationsFrom;
        public override IEnumerable<InterPersonalRelation.To.Complete> InterPersonalRelationsTo => NewInterPersonalRelationsTo;
        public List<CompletedPersonPoliticalEntityRelation> NewPersonPoliticalEntityRelations { get; } = new();
        public List<InterPersonalRelation.From.Complete> NewInterPersonalRelationsFrom { get; set; } = new();
        public List<InterPersonalRelation.To.Complete> NewInterPersonalRelationsTo { get; set; } = new();
        public List<CompletedNewPersonOrganizationRelationForPerson> NewPersonOrganizationRelations { get; } = new();
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