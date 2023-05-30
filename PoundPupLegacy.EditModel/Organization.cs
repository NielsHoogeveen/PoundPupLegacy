using static PoundPupLegacy.EditModel.InterOrganizationalRelation.InterOrganizationalRelationFrom;
using static PoundPupLegacy.EditModel.InterOrganizationalRelation.InterOrganizationalRelationTo;

namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Organization.ExistingOrganization))]
public partial class ExistingOrganizationJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Organization.NewOrganization))]
public partial class NewOrganizationJsonContext : JsonSerializerContext { }

public abstract record Organization : Locatable, ResolvedNode
{
    private Organization() { }
    public abstract T Match<T>(Func<ExistingOrganization, T> existingItem, Func<NewOrganization, T> newItem);
    public abstract void Match(Action<ExistingOrganization> existingItem, Action<NewOrganization> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public required OrganizationDetails OrganizationDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations { get; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }
    public abstract IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations { get; }
    public required List<OrganizationPoliticalEntityRelationTypeListItem> OrganizationPoliticalEntityRelationTypes { get; init; }
    public abstract IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom { get; }
    public abstract IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo { get; }
    public sealed record ExistingOrganization : Organization, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        private List<ExistingOrganizationPoliticalEntityRelation> existingOrganizationPoliticalEntityRelations = new();
        public List<ExistingOrganizationPoliticalEntityRelation> ExistingOrganizationPoliticalEntityRelations {
            get => existingOrganizationPoliticalEntityRelations;
            init {
                if (value is not null) {
                    existingOrganizationPoliticalEntityRelations = value;
                }
            }
        }
        public override IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => GetOrganizationPoliticalEntityRelations();
        private IEnumerable<CompletedOrganizationPoliticalEntityRelation> GetOrganizationPoliticalEntityRelations()
        {
            foreach (var elem in ExistingOrganizationPoliticalEntityRelations) {
                yield return elem;
            }
            foreach (var elem in NewOrganizationPoliticalEntityRelations) {
                yield return elem;
            }
        }
        private List<ExistingInterOrganizationalRelationFrom> existingInterOrganizationalRelationsFrom = new();
        public List<ExistingInterOrganizationalRelationFrom> ExistingInterOrganizationalRelationsFrom {
            get => existingInterOrganizationalRelationsFrom;
            init {
                if (value is not null) {
                    existingInterOrganizationalRelationsFrom = value;
                }
            }
        }
        private List<ExistingInterOrganizationalRelationTo> existingInterOrganizationalRelationsTo = new();
        public List<ExistingInterOrganizationalRelationTo> ExistingInterOrganizationalRelationsTo {
            get => existingInterOrganizationalRelationsTo;
            init {
                if (value is not null) {
                    existingInterOrganizationalRelationsTo = value;
                }
            }
        }
        public override IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom => GetInterOrganizationalRelationsFrom();
        private IEnumerable<CompletedInterOrganizationalRelationFrom> GetInterOrganizationalRelationsFrom()
        {
            foreach (var elem in ExistingInterOrganizationalRelationsFrom) {
                yield return elem;
            }
            foreach (var elem in NewInterOrganizationalRelationsFrom) {
                yield return elem;
            }
        }
        public override IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo => GetInterOrganizationalRelationsTo();
        private IEnumerable<CompletedInterOrganizationalRelationTo> GetInterOrganizationalRelationsTo()
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
        public List<CompletedOrganizationPoliticalEntityRelation> NewOrganizationPoliticalEntityRelations { get; } = new();
        public List<CompletedInterOrganizationalRelationFrom> NewInterOrganizationalRelationsFrom { get; } = new();
        public List<CompletedInterOrganizationalRelationTo> NewInterOrganizationalRelationsTo { get; } = new();
        public List<CompletedNewPersonOrganizationRelationForOrganization> NewPersonOrganizationRelations { get; } = new();
        public override T Match<T>(Func<ExistingOrganization, T> existingItem, Func<NewOrganization, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingOrganization> existingItem, Action<NewOrganization> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewOrganization : Organization, NewLocatable, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override IEnumerable<CompletedOrganizationPoliticalEntityRelation> OrganizationPoliticalEntityRelations => NewOrganizationPoliticalEntityRelations;
        public override IEnumerable<CompletedInterOrganizationalRelationFrom> InterOrganizationalRelationsFrom => NewInterOrganizationalRelationsFrom;
        public override IEnumerable<CompletedInterOrganizationalRelationTo> InterOrganizationalRelationsTo => NewInterOrganizationalRelationsTo;
        public override IEnumerable<CompletedPersonOrganizationRelationForOrganization> PersonOrganizationRelations => NewPersonOrganizationRelations;
        public List<CompletedOrganizationPoliticalEntityRelation> NewOrganizationPoliticalEntityRelations { get; } = new();
        public List<CompletedInterOrganizationalRelationFrom> NewInterOrganizationalRelationsFrom { get; } = new();
        public List<CompletedInterOrganizationalRelationTo> NewInterOrganizationalRelationsTo { get; } = new();
        public List<CompletedNewPersonOrganizationRelationForOrganization> NewPersonOrganizationRelations { get; } = new();
        public override T Match<T>(Func<ExistingOrganization, T> existingItem, Func<NewOrganization, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingOrganization> existingItem, Action<NewOrganization> newItem)
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
