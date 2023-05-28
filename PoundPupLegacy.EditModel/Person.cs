namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Person.ExistingPerson))]
public partial class ExistingPersonJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(Person.NewPerson))]
public partial class NewPersonJsonContext : JsonSerializerContext { }

public abstract record Person : Locatable, ResolvedNode
{
    private Person() { }
    public abstract T Match<T>(Func<ExistingPerson, T> existingItem, Func<NewPerson, T> newItem);
    public abstract void Match(Action<ExistingPerson> existingItem, Action<NewPerson> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public required NodeDetails NodeDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract TenantNodeDetails TenantNodeDetails { get; }
    public abstract IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations { get; }
    public required List<PersonPoliticalEntityRelationTypeListItem> PersonPoliticalEntityRelationTypes { get; init; }
    public abstract IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom { get; }
    public abstract IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo { get; }
    public required List<InterPersonalRelationTypeListItem> InterPersonalRelationTypes { get; init; }
    public abstract IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations { get; }
    public required List<PersonOrganizationRelationTypeListItem> PersonOrganizationRelationTypes { get; init; }
    public sealed record ExistingPerson : Person, ExistingNode, ExistingLocatable
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
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
        private List<ExistingInterPersonalRelationFrom> existingInterPersonalRelationsFrom = new();
        public List<ExistingInterPersonalRelationFrom> ExistingInterPersonalRelationsFrom {
            get => existingInterPersonalRelationsFrom;
            init {
                if (value is not null) {
                    existingInterPersonalRelationsFrom = value;
                }
            }
        }
        private List<ExistingInterPersonalRelationTo> existingInterPersonalRelationsTo = new();
        public List<ExistingInterPersonalRelationTo> ExistingInterPersonalRelationsTo {
            get => existingInterPersonalRelationsTo;
            init {
                if (value is not null) {
                    existingInterPersonalRelationsTo = value;
                }
            }
        }
        public override IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom => GetInterPersonalRelationsFrom();
        private IEnumerable<CompletedInterPersonalRelationFrom> GetInterPersonalRelationsFrom()
        {
            foreach (var elem in ExistingInterPersonalRelationsFrom) {
                yield return elem;
            }
            foreach (var elem in NewInterPersonalRelationsFrom) {
                yield return elem;
            }
        }
        public override IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo => GetInterPersonalRelationsTo();
        private IEnumerable<CompletedInterPersonalRelationTo> GetInterPersonalRelationsTo()
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
        public List<CompletedInterPersonalRelationFrom> NewInterPersonalRelationsFrom { get; set; } = new();
        public List<CompletedInterPersonalRelationTo> NewInterPersonalRelationsTo { get; set; } = new();
        public List<CompletedNewPersonOrganizationRelationForPerson> NewPersonOrganizationRelations { get; } = new();
        public override T Match<T>(Func<ExistingPerson, T> existingItem, Func<NewPerson, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingPerson> existingItem, Action<NewPerson> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewPerson : Person, NewLocatable, ResolvedNewNode
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override IEnumerable<CompletedPersonPoliticalEntityRelation> PersonPoliticalEntityRelations => NewPersonPoliticalEntityRelations;
        public override IEnumerable<CompletedPersonOrganizationRelationForPerson> PersonOrganizationRelations => NewPersonOrganizationRelations;
        public override IEnumerable<CompletedInterPersonalRelationFrom> InterPersonalRelationsFrom => NewInterPersonalRelationsFrom;
        public override IEnumerable<CompletedInterPersonalRelationTo> InterPersonalRelationsTo => NewInterPersonalRelationsTo;
        public List<CompletedPersonPoliticalEntityRelation> NewPersonPoliticalEntityRelations { get; } = new();
        public List<CompletedInterPersonalRelationFrom> NewInterPersonalRelationsFrom { get; set; } = new();
        public List<CompletedInterPersonalRelationTo> NewInterPersonalRelationsTo { get; set; } = new();
        public List<CompletedNewPersonOrganizationRelationForPerson> NewPersonOrganizationRelations { get; } = new();
        public override T Match<T>(Func<ExistingPerson, T> existingItem, Func<NewPerson, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingPerson> existingItem, Action<NewPerson> newItem)
        {
            newItem(this);
        }
    }
}