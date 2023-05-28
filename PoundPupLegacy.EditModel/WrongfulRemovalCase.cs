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
    public required NodeDetails NodeDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract TenantNodeDetails TenantNodeDetails { get; }
    public sealed record ExistingWrongfulRemovalCase : WrongfulRemovalCase, ExistingNode
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
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
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
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
