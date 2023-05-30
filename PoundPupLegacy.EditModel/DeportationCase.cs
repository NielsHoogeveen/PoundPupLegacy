namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DeportationCase.ToUpdate), TypeInfoPropertyName = "DeportationCaseToUpdate")]
public partial class ExistingDeportationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(DeportationCase.ToCreate))]
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
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
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
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
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

