namespace PoundPupLegacy.CreateModel;

public abstract record DeportationCase : Case
{
    private DeportationCase() { }
    public required DeportationCaseDetails DeportationCaseDetails { get; init; }
    public abstract CaseDetails CaseDetails { get; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<DeportationCaseToCreate, T> create, Func<DeportationCaseToUpdate, T> update);
    public abstract void Match(Action<DeportationCaseToCreate> create, Action<DeportationCaseToUpdate> update);

    public sealed record DeportationCaseToCreate : DeportationCase, CaseToCreate
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
        public override T Match<T>(Func<DeportationCaseToCreate, T> create, Func<DeportationCaseToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<DeportationCaseToCreate> create, Action<DeportationCaseToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record DeportationCaseToUpdate : DeportationCase, CaseToUpdate
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
        public override T Match<T>(Func<DeportationCaseToCreate, T> create, Func<DeportationCaseToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<DeportationCaseToCreate> create, Action<DeportationCaseToUpdate> update)
        {
            update(this);
        }
    }
}

public sealed record DeportationCaseDetails
{
    public required int? SubdivisionIdFrom { get; init; }
    public required int? CountryIdTo { get; init; }
}
