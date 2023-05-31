namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DeportationCase.ToUpdate), TypeInfoPropertyName = "DeportationCaseToUpdate")]
[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]
[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(DeportationCase.ToCreate))]
[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class NewDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(DeportationCaseDetails))]
public partial class DeportationCaseDetailsJsonContext : JsonSerializerContext { }

public abstract record DeportationCase : Case, ResolvedNode
{
    private DeportationCase() { }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public required DeportationCaseDetails DeportationCaseDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }

    public sealed record ToUpdate : DeportationCase, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ForUpdate ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate : DeportationCase, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }
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

public sealed record DeportationCaseDetails
{
    public SubdivisionListItem? SubdivisionFrom { get; set; }
    public CountryListItem? CountryTo { get; set; }
}

