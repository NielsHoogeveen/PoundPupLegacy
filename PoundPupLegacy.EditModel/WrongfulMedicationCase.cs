namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(WrongfulMedicationCase.ExistingWrongfulMedicationCase))]
public partial class ExistingWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(WrongfulMedicationCase.NewWrongfulMedicationCase))]
public partial class NewWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

public abstract record WrongfulMedicationCase : Case, ResolvedNode
{
    private WrongfulMedicationCase() { }
    public abstract T Match<T>(Func<ExistingWrongfulMedicationCase, T> existingItem, Func<NewWrongfulMedicationCase, T> newItem);
    public abstract void Match(Action<ExistingWrongfulMedicationCase> existingItem, Action<NewWrongfulMedicationCase> newItem);
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public required NodeDetails NodeDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract TenantNodeDetails TenantNodeDetails { get; }

    public sealed record ExistingWrongfulMedicationCase : WrongfulMedicationCase, ExistingNode
    {
        public override TenantNodeDetails TenantNodeDetails => ExistingTenantNodeDetails;
        public required TenantNodeDetails.ExistingTenantNodeDetails ExistingTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingWrongfulMedicationCase, T> existingItem, Func<NewWrongfulMedicationCase, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingWrongfulMedicationCase> existingItem, Action<NewWrongfulMedicationCase> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewWrongfulMedicationCase : WrongfulMedicationCase, ResolvedNewNode
    {
        public override TenantNodeDetails TenantNodeDetails => NewTenantNodeDetails;
        public required TenantNodeDetails.NewTenantNodeDetails NewTenantNodeDetails { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override T Match<T>(Func<ExistingWrongfulMedicationCase, T> existingItem, Func<NewWrongfulMedicationCase, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingWrongfulMedicationCase> existingItem, Action<NewWrongfulMedicationCase> newItem)
        {
            newItem(this);
        }
    }
}
