namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Organization.ToUpdate), TypeInfoPropertyName ="OrganizationToUpdate")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete), TypeInfoPropertyName = "InterOrganizationalRelationFromComplete")]
[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationIterableFromComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListFromComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete), TypeInfoPropertyName = "InterOrganizationalRelationToComplete")]
[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListToComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterOrganizationalRelationListFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteResolvedToUpdate")]
public partial class ExistingOrganizationJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Organization.ToCreate), TypeInfoPropertyName = "OrganizationToCreate")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete), TypeInfoPropertyName = "InterOrganizationalRelationFromComplete")]
[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationIterableFromComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListFromComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete), TypeInfoPropertyName = "InterOrganizationalRelationToComplete")]
[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListToComplete")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationComplete")]
[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationEnumerableComplete")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationListComplete")]

public partial class NewOrganizationJsonContext : JsonSerializerContext { }

public abstract record Organization : Locatable, ResolvedNode
{
    private Organization() { }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public required OrganizationDetails OrganizationDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    [JsonIgnore]
    public abstract IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations { get; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }
    [JsonIgnore]
    public abstract IEnumerable<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations { get; }
    public required List<OrganizationPoliticalEntityRelationTypeListItem> OrganizationPoliticalEntityRelationTypes { get; init; }

    [JsonIgnore] 
    public abstract IEnumerable<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom { get; }

    [JsonIgnore] 
    public abstract IEnumerable<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo { get; }
    public sealed record ToUpdate : Organization, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        private List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> existingOrganizationPoliticalEntityRelations = new();
        public List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> ExistingOrganizationPoliticalEntityRelations {
            get => existingOrganizationPoliticalEntityRelations;
            init {
                if (value is not null) {
                    existingOrganizationPoliticalEntityRelations = value;
                }
            }
        }
        [JsonIgnore]
        public override IEnumerable<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations => GetOrganizationPoliticalEntityRelations();
        private IEnumerable<OrganizationPoliticalEntityRelation.Complete> GetOrganizationPoliticalEntityRelations()
        {
            foreach (var elem in ExistingOrganizationPoliticalEntityRelations) {
                yield return elem;
            }
            foreach (var elem in NewOrganizationPoliticalEntityRelations) {
                yield return elem;
            }
        }
        private List<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate> existingInterOrganizationalRelationsFrom = new();
        public List<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate> ExistingInterOrganizationalRelationsFrom {
            get => existingInterOrganizationalRelationsFrom;
            init {
                if (value is not null) {
                    existingInterOrganizationalRelationsFrom = value;
                }
            }
        }
        private List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate> existingInterOrganizationalRelationsTo = new();
        public List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate> ExistingInterOrganizationalRelationsTo {
            get => existingInterOrganizationalRelationsTo;
            init {
                if (value is not null) {
                    existingInterOrganizationalRelationsTo = value;
                }
            }
        }
        [JsonIgnore]
        public override IEnumerable<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom => GetInterOrganizationalRelationsFrom();
        private IEnumerable<InterOrganizationalRelation.From.Complete> GetInterOrganizationalRelationsFrom()
        {
            foreach (var elem in ExistingInterOrganizationalRelationsFrom) {
                yield return elem;
            }
            foreach (var elem in NewInterOrganizationalRelationsFrom) {
                yield return elem;
            }
        }
        [JsonIgnore]
        public override IEnumerable<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo => GetInterOrganizationalRelationsTo();
        private IEnumerable<InterOrganizationalRelation.To.Complete> GetInterOrganizationalRelationsTo()
        {
            foreach (var elem in ExistingInterOrganizationalRelationsTo) {
                yield return elem;
            }
            foreach (var elem in NewInterOrganizationalRelationsTo) {
                yield return elem;
            }
        }
        private List<ExistingPersonOrganizationRelationForOrganization> existingPersonOrganizationRelations = new();
        public List<ExistingPersonOrganizationRelationForOrganization> ExistingPersonOrganizationRelations {
            get => existingPersonOrganizationRelations;
            init {
                if (value is not null) {
                    existingPersonOrganizationRelations = value;
                }
            }
        }
        [JsonIgnore]
        public override IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => GetPersonOrganizationRelations();
        private IEnumerable<CompletedPersonOrganizationRelationForOrganization> GetPersonOrganizationRelations()
        {
            foreach (var elem in ExistingPersonOrganizationRelations) {
                yield return elem;
            }
            foreach (var elem in NewPersonOrganizationRelations) {
                yield return elem;
            }
        }
        [JsonIgnore]
        public List<OrganizationPoliticalEntityRelation.Complete> NewOrganizationPoliticalEntityRelations { get; } = new();
        [JsonIgnore]
        public List<InterOrganizationalRelation.From.Complete> NewInterOrganizationalRelationsFrom { get; } = new();
        [JsonIgnore]
        public List<InterOrganizationalRelation.To.Complete> NewInterOrganizationalRelationsTo { get; } = new();
        [JsonIgnore]
        public List<CompletedNewPersonOrganizationRelationForOrganization> NewPersonOrganizationRelations { get; } = new();
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate : Organization, NewLocatable, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        [JsonIgnore]
        public override IEnumerable<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations => NewOrganizationPoliticalEntityRelations;
        [JsonIgnore]
        public override IEnumerable<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom => NewInterOrganizationalRelationsFrom;
        [JsonIgnore]
        public override IEnumerable<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo => NewInterOrganizationalRelationsTo;
        [JsonIgnore]
        public override IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => NewPersonOrganizationRelations;
        public List<OrganizationPoliticalEntityRelation.Complete> NewOrganizationPoliticalEntityRelations { get; } = new();
        [JsonIgnore]
        public List<InterOrganizationalRelation.From.Complete> NewInterOrganizationalRelationsFrom { get; } = new();
        [JsonIgnore] 
        public List<InterOrganizationalRelation.To.Complete> NewInterOrganizationalRelationsTo { get; } = new();
        [JsonIgnore] 
        public List<CompletedNewPersonOrganizationRelationForOrganization> NewPersonOrganizationRelations { get; } = new();
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

public sealed record OrganizationDetails
{
    public required string? WebSiteUrl { get; set; }
    public required string? EmailAddress { get; set; }
    public required FuzzyDate? Establishment { get; set; }
    public required FuzzyDate? Termination { get; set; }
    public required List<OrganizationOrganizationType> OrganizationOrganizationTypes { get; init; }
    public required List<OrganizationType> OrganizationTypes { get; init; }
    public required List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes { get; init; }
    public required OrganizationItem OrganizationItem { get; init; }
}
