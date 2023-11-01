namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(UnresolvedChildTraffickingCase.ToUpdate), TypeInfoPropertyName = "ChildTraffickingCaseToUpdate")]

[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]
[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]

[JsonSerializable(typeof(Tenant.ToUpdate), TypeInfoPropertyName = "TenantToUpdate")]
[JsonSerializable(typeof(List<Tenant.ToUpdate>), TypeInfoPropertyName = "TenantListToUpdate")]

[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]

[JsonSerializable(typeof(Tags.ToUpdate), TypeInfoPropertyName = "TagsToUpdate")]
[JsonSerializable(typeof(List<Tags.ToUpdate>), TypeInfoPropertyName = "TagsListToUpdate")]
public partial class ChildTraffickingCaseToUpdateJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(UnresolvedChildTraffickingCase.ToCreate), TypeInfoPropertyName = "ChildTraffickingCaseToCreate")]
[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]

[JsonSerializable(typeof(Tenant.ToCreate), TypeInfoPropertyName = "TenantToCreate")]
[JsonSerializable(typeof(List<Tenant.ToCreate>), TypeInfoPropertyName = "TenantListToCreate")]

[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]

[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(List<Tags.ToCreate>), TypeInfoPropertyName = "TagsListToCreate")]
public partial class ChildTraffickingCaseToCreateJsonContext : JsonSerializerContext { }


public abstract record ChildTraffickingCase : Case, ResolvedNode, Node<ChildTraffickingCase.ToUpdate, ChildTraffickingCase.ToCreate>, Resolver<ChildTraffickingCase.ToUpdate, ChildTraffickingCase.ToCreate, Unit>
{
    private ChildTraffickingCase() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public abstract T Match<T>(
        Func<ToUpdate, T> existingItem, 
        Func<ToCreate, T> newItem
    );
    public abstract void Match(
        Action<ToUpdate> existingItem, 
        Action<ToCreate> newItem
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
            Func<ToCreate, T> newItem
        )
        {
            return existingItem(this);
        }
        public override void Match(
            Action<ToUpdate> existingItem,
            Action<ToCreate> newItem
        )
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate : ChildTraffickingCase, NewLocatable, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }

        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => ResolvedChildTraffickingCaseDetails;
        public required ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails ResolvedChildTraffickingCaseDetails { get; init; }
        public override T Match<T>(
            Func<ToUpdate, T> existingItem,
            Func<ToCreate, T> newItem
        )
        {
            return newItem(this);
        }
        public override void Match(
            Action<ToUpdate> existingItem,
            Action<ToCreate> newItem
        )
        {
            newItem(this);
        }
    }
}
public abstract record UnresolvedChildTraffickingCase : Case, Node<UnresolvedChildTraffickingCase.ToUpdate, UnresolvedChildTraffickingCase.ToCreate>, Resolver<ChildTraffickingCase.ToUpdate, ChildTraffickingCase.ToCreate, CountryListItem>
{
    private UnresolvedChildTraffickingCase() { }
    public abstract Node<ChildTraffickingCase.ToUpdate, ChildTraffickingCase.ToCreate> Resolve(CountryListItem countryListItem);

    public abstract T Match<T>(
        Func<ToUpdate, T> existingItem,
        Func<ToCreate, T> newItem
    );
    public abstract void Match(
        Action<ToUpdate> existingItem,
        Action<ToCreate> newItem
    );

    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract ChildTraffickingCaseDetails ChildTraffickingCaseDetails { get; }
    public sealed record ToUpdate : UnresolvedChildTraffickingCase, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => ResolvedChildTraffickingCaseDetails;
        public override Node<ChildTraffickingCase.ToUpdate, ChildTraffickingCase.ToCreate> Resolve(CountryListItem countryFrom)
        {
            return new ChildTraffickingCase.ToUpdate {
                CaseDetails = CaseDetails,
                ResolvedChildTraffickingCaseDetails = new ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails {
                    CountryFromExisting = countryFrom,
                    NumberOfChildrenInvolved = ChildTraffickingCaseDetails.NumberOfChildrenInvolved
                },
                NameableDetails = NameableDetails,
                NodeDetailsForUpdate = NodeDetailsForUpdate,
                LocatableDetailsForUpdate = LocatableDetailsForUpdate,
                NodeIdentification = NodeIdentification
            };
        }
        public required ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails ResolvedChildTraffickingCaseDetails { get; init; }
        public override T Match<T>(
            Func<ToUpdate, T> existingItem,
            Func<ToCreate, T> newItem
        )
        {
            return existingItem(this);
        }
        public override void Match(
            Action<ToUpdate> existingItem,
            Action<ToCreate> newItem
        )
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate : UnresolvedChildTraffickingCase, NewLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }

        public override ChildTraffickingCaseDetails ChildTraffickingCaseDetails => NewChildTraffickingCaseDetails;
        public required ChildTraffickingCaseDetails.UnresolvedChildTraffickingCaseDetails NewChildTraffickingCaseDetails { get; init; }
        public override Node<ChildTraffickingCase.ToUpdate, ChildTraffickingCase.ToCreate> Resolve(CountryListItem countryFrom)
        {
            return new ChildTraffickingCase.ToCreate {
                CaseDetails = CaseDetails,
                ResolvedChildTraffickingCaseDetails = new ChildTraffickingCaseDetails.ResolvedChildTraffickingCaseDetails {
                    CountryFromExisting = countryFrom,
                    NumberOfChildrenInvolved = ChildTraffickingCaseDetails.NumberOfChildrenInvolved
                },
                NameableDetails = NameableDetails,
                NodeDetailsForCreate = NodeDetailsForCreate,
                LocatableDetailsForCreate = LocatableDetailsForCreate,
            };
        }
        public override T Match<T>(
            Func<ToUpdate, T> existingItem,
            Func<ToCreate, T> newItem
        )
        {
            return newItem(this);
        }
        public override void Match(
            Action<ToUpdate> existingItem,
            Action<ToCreate> newItem
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

    public abstract CountryListItem? CountryFrom { get; }
    public sealed record UnresolvedChildTraffickingCaseDetails: ChildTraffickingCaseDetails
    {
        public override CountryListItem? CountryFrom => CountryFromNew;
        public required CountryListItem? CountryFromNew { get; set; }
    }
    public sealed record ResolvedChildTraffickingCaseDetails : ChildTraffickingCaseDetails
    {
        public override CountryListItem? CountryFrom => CountryFromExisting;
        public required CountryListItem CountryFromExisting { get; set; }
    }
}