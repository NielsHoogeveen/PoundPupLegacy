namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DeportationCase.ExistingDeportationCase))]
public partial class ExistingDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(DeportationCase.NewDeportationCase))]
public partial class NewDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(DeportationCaseDetails))]
public partial class DeportationCaseDetailsJsonContext : JsonSerializerContext { }

public abstract record DeportationCase : Case, ResolvedNode
{
    private DeportationCase() { }
    public abstract T Match<T>(Func<ExistingDeportationCase, T> existingItem, Func<NewDeportationCase, T> newItem);
    public abstract void Match(Action<ExistingDeportationCase> existingItem, Action<NewDeportationCase> newItem);
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public required NodeDetails NodeDetails { get; init; }
    public required DeportationCaseDetails DeportationCaseDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract TenantNodeDetails TenantNodeDetails { get; }

    public sealed record ExistingDeportationCase : DeportationCase, ExistingNode
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingDeportationCase, T> existingItem, Func<NewDeportationCase, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingDeportationCase> existingItem, Action<NewDeportationCase> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewDeportationCase : DeportationCase, ResolvedNewNode
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override T Match<T>(Func<ExistingDeportationCase, T> existingItem, Func<NewDeportationCase, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingDeportationCase> existingItem, Action<NewDeportationCase> newItem)
        {
            newItem(this);
        }

    }
}

public sealed record DeportationCaseDetails
{
    public SubdivisionListItem? SubdivisionFrom { get; set; }
    public CountryListItem? CountryTo { get; set; }
}

