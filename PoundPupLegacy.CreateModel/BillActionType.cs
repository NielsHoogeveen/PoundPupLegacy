namespace PoundPupLegacy.CreateModel;

public abstract record BillActionType : Nameable
{
    private BillActionType() { }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<BillActionTypeToCreate, T> create, Func<BillActionTypeToUpdate, T> update);
    public abstract void Match(Action<BillActionTypeToCreate> create, Action<BillActionTypeToUpdate> update);

    public sealed record BillActionTypeToCreate : BillActionType, NameableToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<BillActionTypeToCreate, T> create, Func<BillActionTypeToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BillActionTypeToCreate> create, Action<BillActionTypeToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BillActionTypeToUpdate : BillActionType, NameableToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BillActionTypeToCreate, T> create, Func<BillActionTypeToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BillActionTypeToCreate> create, Action<BillActionTypeToUpdate> update)
        {
            update(this);
        }
    }
}
