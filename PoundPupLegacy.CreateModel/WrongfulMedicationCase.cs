namespace PoundPupLegacy.CreateModel;

public abstract record WrongfulMedicationCase : Case
{
    private WrongfulMedicationCase() { }
    public abstract CaseDetails CaseDetails { get; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract Identification Identification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<ToCreate, T> create, Func<WrongfulMedicationCaseToUpdate, T> update);
    public abstract void Match(Action<ToCreate> create, Action<WrongfulMedicationCaseToUpdate> update);

    public sealed record ToCreate : WrongfulMedicationCase, CaseToCreate
    {
        public override Identification Identification => IdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override CaseDetails CaseDetails => CaseDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required Identification.Possible IdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public override T Match<T>(Func<ToCreate, T> create, Func<WrongfulMedicationCaseToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<ToCreate> create, Action<WrongfulMedicationCaseToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record WrongfulMedicationCaseToUpdate : WrongfulMedicationCase, CaseToUpdate
    {
        public required Identification.Certain IdentificationCertain { get; init; }
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override Identification Identification => IdentificationCertain;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override CaseDetails CaseDetails => CaseDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<ToCreate, T> create, Func<WrongfulMedicationCaseToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<ToCreate> create, Action<WrongfulMedicationCaseToUpdate> update)
        {
            update(this);
        }
    }
}
