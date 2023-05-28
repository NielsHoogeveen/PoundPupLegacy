namespace PoundPupLegacy.CreateModel;

public abstract record DisruptedPlacementCase : Case
{
    private DisruptedPlacementCase() { }
    public abstract CaseDetails CaseDetails { get; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<DisruptedPlacementCaseToCreate, T> create, Func<DisruptedPlacementCaseToUpdate, T> update);
    public abstract void Match(Action<DisruptedPlacementCaseToCreate> create, Action<DisruptedPlacementCaseToUpdate> update);

    public sealed record DisruptedPlacementCaseToCreate : DisruptedPlacementCase, CaseToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public override CaseDetails CaseDetails => CaseDetailsForCreate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public required CaseDetails.CaseDetailsForCreate CaseDetailsForCreate { get; init; }
        public required LocatableDetails.LocatableDetailsForCreate LocatableDetailsForCreate { get; init; }
        public override T Match<T>(Func<DisruptedPlacementCaseToCreate, T> create, Func<DisruptedPlacementCaseToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<DisruptedPlacementCaseToCreate> create, Action<DisruptedPlacementCaseToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record DisruptedPlacementCaseToUpdate : DisruptedPlacementCase, CaseToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public override CaseDetails CaseDetails => CaseDetailsForUpdate;
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public required CaseDetails.CaseDetailsForUpdate CaseDetailsForUpdate { get; init; }
        public required LocatableDetails.LocatableDetailsForUpdate LocatableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<DisruptedPlacementCaseToCreate, T> create, Func<DisruptedPlacementCaseToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<DisruptedPlacementCaseToCreate> create, Action<DisruptedPlacementCaseToUpdate> update)
        {
            update(this);
        }
    }
}
