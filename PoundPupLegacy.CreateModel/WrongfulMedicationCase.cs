namespace PoundPupLegacy.CreateModel;

public abstract record WrongfulMedicationCase : Case
{
    private WrongfulMedicationCase() { }

    public sealed record ToCreate : WrongfulMedicationCase, CaseToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetails { get; init; }
    }
    public sealed record WrongfulMedicationCaseToUpdate : WrongfulMedicationCase, CaseToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetails { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetails { get; init; }
    }
}
