namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Organization.ToUpdate), TypeInfoPropertyName ="OrganizationToUpdate")]

[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationIterableFromComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete), TypeInfoPropertyName = "InterOrganizationalRelationFromComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListFromComplete")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete), TypeInfoPropertyName = "InterOrganizationalRelationToComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListToComplete")]

[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterOrganizationalRelationListFromCompleteResolvedToUpdate")]

[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteResolvedToUpdate")]

[JsonSerializable(typeof(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationEnumerableForOrganizationComplete")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForOrganization.Complete), TypeInfoPropertyName = "PersonOrganizationRelationForOrganizationComplete")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForOrganization.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationListForOrganizationComplete")]

[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationEnumerableCompleteResolvedToUpdate")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationListCompleteResolvedToUpdate")]

[JsonSerializable(typeof(PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate), TypeInfoPropertyName = "PersonOrganizationRelationForOrganizationCompleteResolvedToCreate")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "PersonOrganizationRelationForOrganizationCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "PersonOrganizationRelationListForOrganizationCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "PersonOrganizationRelationListForOrganizationCompleteResolvedToCreate")]

[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]

[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]

[JsonSerializable(typeof(TenantNodeDetails.ForUpdate), TypeInfoPropertyName = "TenantNodeDetailsForUpdate")]
[JsonSerializable(typeof(TenantNodeDetails.ForCreate), TypeInfoPropertyName = "TenantNodeDetailsForCreate")]

[JsonSerializable(typeof(TenantNodeDetails.ForUpdate), TypeInfoPropertyName = "TenantNodeDetailsForUpdate")]

[JsonSerializable(typeof(Tags.ToUpdate), TypeInfoPropertyName = "TagsToUpdate")]
[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(List<Tags.ToUpdate>), TypeInfoPropertyName = "TagsListToUpdate")]

public partial class OrganizationToUpdateJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Organization.ToCreate), TypeInfoPropertyName = "OrganizationToCreate")]

[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationIterableFromComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete), TypeInfoPropertyName = "InterOrganizationalRelationFromComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListFromComplete")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete), TypeInfoPropertyName = "InterOrganizationalRelationToComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListToComplete")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteToCreateForNewOrganization")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableFromCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteFromCreateForNewOrganization")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteFromCreateForNewOrganization")]

[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationEnumerableComplete")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationComplete")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationListComplete")]

[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteToCreateEnumeranbleForNewOrganization")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteListToCreateForNewOrganization")]

[JsonSerializable(typeof(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationEnumerableForOrganizationComplete")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForOrganization.Complete), TypeInfoPropertyName = "PersonOrganizationRelationForOrganizationComplete")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForOrganization.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationListForOrganizationComplete")]

[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
[JsonSerializable(typeof(TenantNodeDetails.ForCreate), TypeInfoPropertyName = "TenantNodeDetailsForCreate")]

[JsonSerializable(typeof(TenantNodeDetails.ForCreate), TypeInfoPropertyName = "TenantNodeDetailsForCreate")]

[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(List<Tags.ToCreate>), TypeInfoPropertyName = "TagsListToCreate")]

public partial class OrganizationToCreateJsonContext : JsonSerializerContext { }


public abstract record Organization : Locatable, ResolvedNode, Node<Organization.ToUpdate, Organization.ToCreate>, Resolver<Organization.ToUpdate, Organization.ToCreate, Unit>
{
    private Organization() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public required OrganizationDetails OrganizationDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    [JsonIgnore]
    public abstract IEnumerable<PersonOrganizationRelation.ForOrganization.Complete> PersonOrganizationRelations { get; }
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
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        private List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> existingOrganizationPoliticalEntityRelations = new();
        public List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> OrganizationPoliticalEntityRelationsToUpdate {
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
            foreach (var elem in OrganizationPoliticalEntityRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in OrganizationPoliticalEntityRelationsToCreate) {
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
        public IEnumerable<InterOrganizationalRelation> InterOrganizationalRelation => GetInterOrganizationalRelations();
        private IEnumerable<InterOrganizationalRelation> GetInterOrganizationalRelations()
        {
            foreach (var elem in InterOrganizationalRelationsFrom) {
                yield return elem;
            }
            foreach (var elem in InterOrganizationalRelationsTo) {
                yield return elem;
            }
        }
        private List<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate> existingPersonOrganizationRelations = new();
        public List<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate> PersonOrganizationRelationsToUpdate {
            get => existingPersonOrganizationRelations;
            init {
                if (value is not null) {
                    existingPersonOrganizationRelations = value;
                }
            }
        }
        [JsonIgnore]
        public override IEnumerable<PersonOrganizationRelation.ForOrganization.Complete> PersonOrganizationRelations => GetPersonOrganizationRelations();
        private IEnumerable<PersonOrganizationRelation.ForOrganization.Complete> GetPersonOrganizationRelations()
        {
            foreach (var elem in PersonOrganizationRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in PersonOrganizationRelationsToCreate) {
                yield return elem;
            }
        }
        [JsonIgnore]
        public List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization> OrganizationPoliticalEntityRelationsToCreate { get; } = new();
        [JsonIgnore]
        public List<InterOrganizationalRelation.From.Complete> NewInterOrganizationalRelationsFrom { get; } = new();
        [JsonIgnore]
        public List<InterOrganizationalRelation.To.Complete> NewInterOrganizationalRelationsTo { get; } = new();
        [JsonIgnore]
        public List<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate> PersonOrganizationRelationsToCreate { get; } = new();
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
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }
        [JsonIgnore]
        public override IEnumerable<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations => OrganizationPoliticalEntityRelationsToCreate;
        [JsonIgnore]
        public override IEnumerable<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom => InterOrganizationalRelationsFromToCreate;
        [JsonIgnore]
        public override IEnumerable<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo => InterOrganizationalRelationsToToCreate;
        [JsonIgnore]
        public override IEnumerable<PersonOrganizationRelation.ForOrganization.Complete> PersonOrganizationRelations => PersonOrganizationRelationsToCreate;
        public List<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization> OrganizationPoliticalEntityRelationsToCreate { get; } = new();
        [JsonIgnore]
        public List<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization> InterOrganizationalRelationsFromToCreate { get; } = new();
        [JsonIgnore] 
        public List<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization> InterOrganizationalRelationsToToCreate { get; } = new();
        [JsonIgnore] 
        public List<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization> PersonOrganizationRelationsToCreate { get; } = new();
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
