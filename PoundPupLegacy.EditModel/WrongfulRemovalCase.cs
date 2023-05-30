namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(WrongfulRemovalCase.ExistingWrongfulRemovalCase))]
public partial class ExistingWrongfulRemovalCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(WrongfulRemovalCase.NewWrongfulRemovalCase))]
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
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
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
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
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
