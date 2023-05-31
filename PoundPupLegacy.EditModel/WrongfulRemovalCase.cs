namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(WrongfulRemovalCase.ExistingWrongfulRemovalCase))]

[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]
[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(WrongfulRemovalCase.NewWrongfulRemovalCase))]

[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class NewWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

public abstract record WrongfulRemovalCase : Case, ResolvedNode
{
    private WrongfulRemovalCase() { }
    public abstract T Match<T>(Func<ExistingWrongfulRemovalCase, T> existingItem, Func<NewWrongfulRemovalCase, T> newItem);
    public abstract void Match(Action<ExistingWrongfulRemovalCase> existingItem, Action<NewWrongfulRemovalCase> newItem);

    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ExistingWrongfulRemovalCase : WrongfulRemovalCase, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ForUpdate ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingWrongfulRemovalCase, T> existingItem, Func<NewWrongfulRemovalCase, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingWrongfulRemovalCase> existingItem, Action<NewWrongfulRemovalCase> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewWrongfulRemovalCase : WrongfulRemovalCase, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.ForCreate NewLocatableDetails { get; init; }
        public override T Match<T>(Func<ExistingWrongfulRemovalCase, T> existingItem, Func<NewWrongfulRemovalCase, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingWrongfulRemovalCase> existingItem, Action<NewWrongfulRemovalCase> newItem)
        {
            newItem(this);
        }
    }
}
