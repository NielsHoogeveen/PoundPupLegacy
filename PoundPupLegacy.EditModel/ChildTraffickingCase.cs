namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ChildTraffickingCase.ExistingChildTraffickingCase))]
public partial class ExistingChildTraffickingCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ChildTraffickingCase.NewChildTraffickingCase))]
public partial class NewChildTraffickingCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails))]
public partial class ResolvedChildTraffickingCaseDetailsJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ChildTraffickingCaseDetails.NewChildTraffickingCaseDetails))]
public partial class NewChildTraffickingCaseDetailsJsonContext : JsonSerializerContext { }

public abstract record ChildTraffickingCase : Case
{
    private ChildTraffickingCase() { }
    public abstract T Match<T>(
        Func<ExistingChildTraffickingCase, T> existingItem, 
        Func<NewChildTraffickingCase, T> newItem,
        Func<ResolvedNewChildTraffickingCase, T> resolvedNewItem
    );
    public abstract void Match(
        Action<ExistingChildTraffickingCase> existingItem, 
        Action<NewChildTraffickingCase> newItem,
        Action<ResolvedNewChildTraffickingCase> resolvedNewItem
    );

    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public required NodeDetails NodeDetails { get; init; }
    
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract TenantNodeDetails TenantNodeDetails { get; }

    public abstract ChildTraffickingCaseDetails ChildTraffickingCaseDetails { get; }
    public sealed record ExistingChildTraffickingCase : ChildTraffickingCase, ExistingLocatable
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => ResolvedChildTraffickingCaseDetails;
        public required ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails ResolvedChildTraffickingCaseDetails { get; init; }
        public override T Match<T>(
            Func<ExistingChildTraffickingCase, T> existingItem,
            Func<NewChildTraffickingCase, T> newItem,
            Func<ResolvedNewChildTraffickingCase, T> resolvedNewItem
        )
        {
            return existingItem(this);
        }
        public override void Match(
            Action<ExistingChildTraffickingCase> existingItem,
            Action<NewChildTraffickingCase> newItem,
            Action<ResolvedNewChildTraffickingCase> resolvedNewItem
        )
        {
            existingItem(this);
        }
    }
    public sealed record ResolvedNewChildTraffickingCase : ChildTraffickingCase, ResolvedNewNode, NewLocatable
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => ResolvedChildTraffickingCaseDetails;
        public required ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails ResolvedChildTraffickingCaseDetails { get; init; }
        public override T Match<T>(
            Func<ExistingChildTraffickingCase, T> existingItem,
            Func<NewChildTraffickingCase, T> newItem,
            Func<ResolvedNewChildTraffickingCase, T> resolvedNewItem
        )
        {
            return resolvedNewItem(this);
        }
        public override void Match(
            Action<ExistingChildTraffickingCase> existingItem,
            Action<NewChildTraffickingCase> newItem,
            Action<ResolvedNewChildTraffickingCase> resolvedNewItem
        )
        {
            resolvedNewItem(this);
        }
    }
    public sealed record NewChildTraffickingCase : ChildTraffickingCase, NewNode, NewLocatable
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => NewChildTraffickingCaseDetails;
        public required ChildTraffickingCaseDetails.NewChildTraffickingCaseDetails NewChildTraffickingCaseDetails { get; init; }
        public ResolvedNewChildTraffickingCase Resolve(CountryListItem countryFrom)
        {
            return new ResolvedNewChildTraffickingCase {
                CaseDetails = CaseDetails,
                ResolvedChildTraffickingCaseDetails = new ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails 
                { 
                    CountryFrom = countryFrom,
                    NumberOfChildrenInvolved = ChildTraffickingCaseDetails.NumberOfChildrenInvolved
                },
                NameableDetails = NameableDetails,
                NodeDetails = NodeDetails,
                NewLocatableDetails = NewLocatableDetails,
                NewTenantNodeDetails = NewTenantNodeDetails,
            };
        }
        public override T Match<T>(
            Func<ExistingChildTraffickingCase, T> existingItem,
            Func<NewChildTraffickingCase, T> newItem,
            Func<ResolvedNewChildTraffickingCase, T> resolvedNewItem
        )
        {
            return newItem(this);
        }
        public override void Match(
            Action<ExistingChildTraffickingCase> existingItem,
            Action<NewChildTraffickingCase> newItem,
            Action<ResolvedNewChildTraffickingCase> resolvedNewItem
        )
        {
            newItem(this);
        }
    }
}

public abstract record ChildTraffickingCaseDetails
{
    private ChildTraffickingCaseDetails() { }
    public required int? NumberOfChildrenInvolved { get; set; }
    public sealed record NewChildTraffickingCaseDetails: ChildTraffickingCaseDetails
    {
        public required CountryListItem? CountryFrom { get; set; }
    }
    public sealed record ResolvedChildTraffickingCaseDetails : ChildTraffickingCaseDetails
    {
        public required CountryListItem CountryFrom { get; set; }
    }
}