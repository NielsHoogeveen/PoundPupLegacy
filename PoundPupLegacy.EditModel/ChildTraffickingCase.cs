namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ChildTraffickingCase.ToUpdate), TypeInfoPropertyName = "ChildTraffickingCaseToUpdate")]
[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]
[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingChildTraffickingCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ChildTraffickingCase.ToCreate.Unresolved))]
[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class NewChildTraffickingCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails))]
public partial class ResolvedChildTraffickingCaseDetailsJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(ChildTraffickingCaseDetails.NewChildTraffickingCaseDetails))]
public partial class NewChildTraffickingCaseDetailsJsonContext : JsonSerializerContext { }

public abstract record ChildTraffickingCase : Case
{
    private ChildTraffickingCase() { }
    public abstract T Match<T>(
        Func<ToUpdate, T> existingItem, 
        Func<ToCreate.Unresolved, T> newItem,
        Func<ToCreate.Resolved, T> resolvedNewItem
    );
    public abstract void Match(
        Action<ToUpdate> existingItem, 
        Action<ToCreate.Unresolved> newItem,
        Action<ToCreate.Resolved> resolvedNewItem
    );

    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract ChildTraffickingCaseDetails ChildTraffickingCaseDetails { get; }
    public sealed record ToUpdate : ChildTraffickingCase, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => ResolvedChildTraffickingCaseDetails;
        public required ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails ResolvedChildTraffickingCaseDetails { get; init; }
        public override T Match<T>(
            Func<ToUpdate, T> existingItem,
            Func<ToCreate.Unresolved, T> newItem,
            Func<ToCreate.Resolved, T> resolvedNewItem
        )
        {
            return existingItem(this);
        }
        public override void Match(
            Action<ToUpdate> existingItem,
            Action<ToCreate.Unresolved> newItem,
            Action<ToCreate.Resolved> resolvedNewItem
        )
        {
            existingItem(this);
        }
    }
    public abstract record ToCreate : ChildTraffickingCase, NewLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }

        public sealed record Resolved : ToCreate, ResolvedNewNode
        {
            public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => ResolvedChildTraffickingCaseDetails;
            public required ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails ResolvedChildTraffickingCaseDetails { get; init; }
            public override T Match<T>(
                Func<ToUpdate, T> existingItem,
                Func<Unresolved, T> newItem,
                Func<Resolved, T> resolvedNewItem
            )
            {
                return resolvedNewItem(this);
            }
            public override void Match(
                Action<ToUpdate> existingItem,
                Action<Unresolved> newItem,
                Action<Resolved> resolvedNewItem
            )
            {
                resolvedNewItem(this);
            }
        }
        public sealed record Unresolved : ToCreate, NewNode
        {
            public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => NewChildTraffickingCaseDetails;
            public required ChildTraffickingCaseDetails.NewChildTraffickingCaseDetails NewChildTraffickingCaseDetails { get; init; }
            public Resolved Resolve(CountryListItem countryFrom)
            {
                return new Resolved {
                    CaseDetails = CaseDetails,
                    ResolvedChildTraffickingCaseDetails = new ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails {
                        CountryFrom = countryFrom,
                        NumberOfChildrenInvolved = ChildTraffickingCaseDetails.NumberOfChildrenInvolved
                    },
                    NameableDetails = NameableDetails,
                    NodeDetailsForCreate = NodeDetailsForCreate,
                    LocatableDetailsForCreate = LocatableDetailsForCreate,
                };
            }
            public override T Match<T>(
                Func<ToUpdate, T> existingItem,
                Func<Unresolved, T> newItem,
                Func<Resolved, T> resolvedNewItem
            )
            {
                return newItem(this);
            }
            public override void Match(
                Action<ToUpdate> existingItem,
                Action<Unresolved> newItem,
                Action<Resolved> resolvedNewItem
            )
            {
                newItem(this);
            }
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