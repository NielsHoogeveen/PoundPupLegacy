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

[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.Resolved.ToCreate), TypeInfoPropertyName = "InterOrganizationalRelationFromCompleteResolvedToCreate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "InterOrganizationalRelationListFromCompleteResolvedToCreate")]

[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.Resolved.ToCreate), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteResolvedToCreate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteResolvedToCreate")]

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

[JsonSerializable(typeof(OrganizationDetails.ForUpdate), TypeInfoPropertyName = "OrganizationDetailsForUpdate")]

[JsonSerializable(typeof(Tags.ToUpdate), TypeInfoPropertyName = "TagsToUpdate")]
[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(List<Tags.ToUpdate>), TypeInfoPropertyName = "TagsListToUpdate")]

public partial class OrganizationToUpdateJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Organization.ToCreate), TypeInfoPropertyName = "OrganizationToCreate")]

[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]

[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationIterableFromComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete), TypeInfoPropertyName = "InterOrganizationalRelationFromComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListFromComplete")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToComplete")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete), TypeInfoPropertyName = "InterOrganizationalRelationToComplete")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete>), TypeInfoPropertyName = "InterOrganizationalRelationListToComplete")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableFromCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteFromCreateForNewOrganization")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteFromCreateForNewOrganization")]

[JsonSerializable(typeof(IEnumerable<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationEnumerableToCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteToCreateForNewOrganization")]

[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationFromCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterOrganizationalRelationListFromCompleteResolvedToUpdate")]

[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteResolvedToUpdate")]

[JsonSerializable(typeof(InterOrganizationalRelation.From.Complete.Resolved.ToCreate), TypeInfoPropertyName = "InterOrganizationalRelationFromCompleteResolvedToCreate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.From.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "InterOrganizationalRelationListFromCompleteResolvedToCreate")]

[JsonSerializable(typeof(InterOrganizationalRelation.To.Complete.Resolved.ToCreate), TypeInfoPropertyName = "InterOrganizationalRelationToCompleteResolvedToCreate")]
[JsonSerializable(typeof(List<InterOrganizationalRelation.To.Complete.Resolved.ToCreate>), TypeInfoPropertyName = "InterOrganizationalRelationListToCompleteResolvedToCreate")]

[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationEnumerableComplete")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationComplete")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationListComplete")]

[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteToCreateEnumeranbleForNewOrganization")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteToCreateForNewOrganization")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteListToCreateForNewOrganization")]

[JsonSerializable(typeof(IEnumerable<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationEnumerableCompleteResolvedToUpdate")]
[JsonSerializable(typeof(OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationCompleteResolvedToUpdate")]
[JsonSerializable(typeof(List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate>), TypeInfoPropertyName = "OrganizationPoliticalEntityRelationListCompleteResolvedToUpdate")]

[JsonSerializable(typeof(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationEnumerableForOrganizationComplete")]
[JsonSerializable(typeof(PersonOrganizationRelation.ForOrganization.Complete), TypeInfoPropertyName = "PersonOrganizationRelationForOrganizationComplete")]
[JsonSerializable(typeof(List<PersonOrganizationRelation.ForOrganization.Complete>), TypeInfoPropertyName = "PersonOrganizationRelationListForOrganizationComplete")]

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

[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(Tags.ToUpdate), TypeInfoPropertyName = "TagsToUpdate")]

[JsonSerializable(typeof(List<Tags.ToUpdate>), TypeInfoPropertyName = "TagsListToUpdate")]
[JsonSerializable(typeof(List<Tags.ToCreate>), TypeInfoPropertyName = "TagsListToCreate")]

[JsonSerializable(typeof(OrganizationDetails.ForCreate), TypeInfoPropertyName = "OrganizationDetailsForCreate")]
[JsonSerializable(typeof(OrganizationDetails.ForUpdate), TypeInfoPropertyName = "OrganizationDetailsForUpdate")]
public partial class OrganizationToCreateJsonContext : JsonSerializerContext { }


public abstract record Organization : Locatable, ResolvedNode, Node<Organization.ToUpdate, Organization.ToCreate>, Resolver<Organization.ToUpdate, Organization.ToCreate, Unit>
{
    private Organization() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract OrganizationDetails OrganizationDetails { get; }
    public sealed record ToUpdate : Organization, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForUpdate;
        public required OrganizationDetails.ForUpdate OrganizationDetailsForUpdate { get; init; }
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
        public override OrganizationDetails OrganizationDetails => OrganizationDetailsForCreate;
        public required OrganizationDetails.ForCreate OrganizationDetailsForCreate { get; init; }
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

public abstract record OrganizationDetails
{
    public required string? WebSiteUrl { get; set; }
    public required string? EmailAddress { get; set; }
    public required FuzzyDate? Establishment { get; set; }
    public required FuzzyDate? Termination { get; set; }

    private List<OrganizationOrganizationType> organizationOrganizationTypes = new List<OrganizationOrganizationType>();
    public required List<OrganizationOrganizationType> OrganizationOrganizationTypes { 
        get => organizationOrganizationTypes; 
        init {
            if (value is not null)
                organizationOrganizationTypes = value;
        } 
    }
    public required List<OrganizationType> OrganizationTypes { get; init; } 
    public required List<InterOrganizationalRelationTypeListItem> InterOrganizationalRelationTypes { get; init; }
    public abstract OrganizationItem OrganizationItem { get; }
    public abstract List<PersonOrganizationRelation.ForOrganization.Complete> PersonOrganizationRelations { get; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }
    public abstract List<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations { get; }
    public required List<OrganizationPoliticalEntityRelationTypeListItem> OrganizationPoliticalEntityRelationTypes { get; init; }

    public abstract List<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom { get; set; }

    public abstract List<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo { get; set; }

    public sealed record ForUpdate : OrganizationDetails
    {
        public required OrganizationListItem OrganizationListItem { get; init; }

        public override OrganizationItem OrganizationItem => OrganizationListItem;

        private List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> existingOrganizationPoliticalEntityRelations = new();
        public List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> OrganizationPoliticalEntityRelationsToUpdate {
            get => existingOrganizationPoliticalEntityRelations;
            init {
                if (value is not null) {
                    existingOrganizationPoliticalEntityRelations = value;
                }
            }
        }
        public override List<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations => GetOrganizationPoliticalEntityRelations().ToList();
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
        public List<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate> InterOrganizationalRelationsFromToUpdate {
            get => existingInterOrganizationalRelationsFrom;
            set {
                if (value is not null) {
                    existingInterOrganizationalRelationsFrom = value;
                }
            }
        }
        private List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate> existingInterOrganizationalRelationsTo = new();
        public List<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate> InterOrganizationalRelationsToToUpdate {
            get => existingInterOrganizationalRelationsTo;
            init {
                if (value is not null) {
                    existingInterOrganizationalRelationsTo = value;
                }
            }
        }

        private List<InterOrganizationalRelation.From.Complete>? interOrganizationalRelationsFrom = null;
        public override List<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom { 
            get { 
                if(interOrganizationalRelationsFrom is null) {
                    interOrganizationalRelationsFrom = GetInterOrganizationalRelationsFrom().ToList();
                }
                return interOrganizationalRelationsFrom;
            } 
            set{
                if (value is not null) {
                    interOrganizationalRelationsFrom = value;
                }
            } 
        }
        private IEnumerable<InterOrganizationalRelation.From.Complete> GetInterOrganizationalRelationsFrom()
        {
            foreach (var elem in InterOrganizationalRelationsFromToUpdate) {
                yield return elem;
            }
            foreach (var elem in InterOrganizationalRelationsFromToCreate) {
                yield return elem;
            }
        }
        private List<InterOrganizationalRelation.To.Complete>? interOrganizationalRelationsTo = null;
        public override List<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo {
            get {
                if (interOrganizationalRelationsTo is null) {
                    interOrganizationalRelationsTo = GetInterOrganizationalRelationsTo().ToList();
                }
                return interOrganizationalRelationsTo;
            }
            set {
                if (value is not null) {
                    interOrganizationalRelationsTo = value;
                }
            }
        }
        private IEnumerable<InterOrganizationalRelation.To.Complete> GetInterOrganizationalRelationsTo()
        {
            foreach (var elem in InterOrganizationalRelationsToToUpdate) {
                yield return elem;
            }
            foreach (var elem in InterOrganizationalRelationsToToCreate) {
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
        public override List<PersonOrganizationRelation.ForOrganization.Complete> PersonOrganizationRelations => GetPersonOrganizationRelations().ToList();
        private IEnumerable<PersonOrganizationRelation.ForOrganization.Complete> GetPersonOrganizationRelations()
        {
            foreach (var elem in PersonOrganizationRelationsToUpdate) {
                yield return elem;
            }
            foreach (var elem in PersonOrganizationRelationsToCreate) {
                yield return elem;
            }
        }
        public List<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization> OrganizationPoliticalEntityRelationsToCreate { get; set; } = new();
        public List<InterOrganizationalRelation.From.Complete.Resolved.ToCreate> InterOrganizationalRelationsFromToCreate { get; set; } = new();
        public List<InterOrganizationalRelation.To.Complete.Resolved.ToCreate> InterOrganizationalRelationsToToCreate { get; set; } = new();
        public List<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate> PersonOrganizationRelationsToCreate { get; set; } = new();

    }

    public sealed record ForCreate: OrganizationDetails
    {
        public required OrganizationName OrganizationName { get; init; }
        public override OrganizationItem OrganizationItem => OrganizationName;
        public override List<OrganizationPoliticalEntityRelation.Complete> OrganizationPoliticalEntityRelations => GetOrganizationPoliticalEntityRelations().ToList();
        private List<InterOrganizationalRelation.From.Complete>? interOrganizationalRelationsFrom = null;
        public override List<InterOrganizationalRelation.From.Complete> InterOrganizationalRelationsFrom {
            get {
                if (interOrganizationalRelationsFrom is null) {
                    interOrganizationalRelationsFrom = GetInterOrganizationalRelationsFrom().ToList();
                }
                return interOrganizationalRelationsFrom;
            }
            set {
                if (value is not null) {
                    interOrganizationalRelationsFrom = value;
                }
            }
        }
        private List<InterOrganizationalRelation.To.Complete>? interOrganizationalRelationsTo = null;
        public override List<InterOrganizationalRelation.To.Complete> InterOrganizationalRelationsTo {
            get {
                if (interOrganizationalRelationsTo is null) {
                    interOrganizationalRelationsTo = GetInterOrganizationalRelationsTo().ToList();
                }
                return interOrganizationalRelationsTo;
            }
            set {
                if (value is not null) {
                    interOrganizationalRelationsTo = value;
                }
            }
        }
        public override List<PersonOrganizationRelation.ForOrganization.Complete> PersonOrganizationRelations => GetPersonOrganizationRelationsToCreate().ToList();
        public List<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization> OrganizationPoliticalEntityRelationsToCreate { get; } = new();
        public List<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization> InterOrganizationalRelationsFromToCreate { get; } = new();
        public List<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization> InterOrganizationalRelationsToToCreate { get; } = new();
        public List<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization> PersonOrganizationRelationsToCreate { get; } = new();
        private IEnumerable<OrganizationPoliticalEntityRelation.Complete> GetOrganizationPoliticalEntityRelations()
        {
            foreach (var elem in OrganizationPoliticalEntityRelationsToCreate) {
                yield return elem;
            }
        }
        private IEnumerable<InterOrganizationalRelation.From.Complete> GetInterOrganizationalRelationsFrom()
        {
            foreach (var elem in InterOrganizationalRelationsFromToCreate) {
                yield return elem;
            }
        }
        private IEnumerable<InterOrganizationalRelation.To.Complete> GetInterOrganizationalRelationsTo()
        {
            foreach (var elem in InterOrganizationalRelationsToToCreate) {
                yield return elem;
            }
        }
        private IEnumerable<PersonOrganizationRelation.ForOrganization.Complete> GetPersonOrganizationRelationsToCreate()
        {
            foreach (var elem in PersonOrganizationRelationsToCreate) {
                yield return elem;
            }
        }
    }
}
