namespace PoundPupLegacy.CreateModel;

public abstract record AbuseCase : Case
{
    private AbuseCase() { }
    public required AbuseCaseDetails AbuseCaseDetails { get; init; }
    public abstract CaseDetails CaseDetails { get; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<AbuseCaseToCreate, T> create, Func<AbuseCaseToUpdate, T> update);
    public abstract void Match(Action<AbuseCaseToCreate> create, Action<AbuseCaseToUpdate> update);

    public sealed record AbuseCaseToCreate : AbuseCase, CaseToCreate
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
        public override T Match<T>(Func<AbuseCaseToCreate, T> create, Func<AbuseCaseToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<AbuseCaseToCreate> create, Action<AbuseCaseToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record AbuseCaseToUpdate : AbuseCase, CaseToUpdate
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
        public override T Match<T>(Func<AbuseCaseToCreate, T> create, Func<AbuseCaseToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<AbuseCaseToCreate> create, Action<AbuseCaseToUpdate> update)
        {
            update(this);
        }
    }
}


public sealed record AbuseCaseDetails
{
    public required int ChildPlacementTypeId { get; init; }
    public required int? FamilySizeId { get; init; }
    public required bool? HomeschoolingInvolved { get; init; }
    public required bool? FundamentalFaithInvolved { get; init; }
    public required bool? DisabilitiesInvolved { get; init; }
    public required List<int> TypeOfAbuseIds { get; init; }
    public required List<int> TypeOfAbuserIds { get; init; }
}
