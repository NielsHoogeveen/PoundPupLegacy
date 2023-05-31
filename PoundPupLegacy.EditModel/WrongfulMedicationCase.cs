namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(WrongfulMedicationCase.ExistingWrongfulMedicationCase))]

[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]

[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(WrongfulMedicationCase.NewWrongfulMedicationCase))]

[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class NewWrongfulMedicationCaseJsonContext : JsonSerializerContext { }

public abstract record WrongfulMedicationCase : Case, ResolvedNode
{
    private WrongfulMedicationCase() { }
    public abstract T Match<T>(Func<ExistingWrongfulMedicationCase, T> existingItem, Func<NewWrongfulMedicationCase, T> newItem);
    public abstract void Match(Action<ExistingWrongfulMedicationCase> existingItem, Action<NewWrongfulMedicationCase> newItem);
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }

    public sealed record ExistingWrongfulMedicationCase : WrongfulMedicationCase, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ForUpdate ExistingLocatableDetails { get; init; }
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
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.ForCreate NewLocatableDetails { get; init; }
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
