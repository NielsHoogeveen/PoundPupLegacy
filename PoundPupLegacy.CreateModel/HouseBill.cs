namespace PoundPupLegacy.CreateModel;

public abstract record HouseBill : Bill
{
    private HouseBill() { }
    public required BillDetails BillDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<HouseBillToCreate, T> create, Func<HouseBillToUpdate, T> update);
    public abstract void Match(Action<HouseBillToCreate> create, Action<HouseBillToUpdate> update);

    public sealed record HouseBillToCreate : HouseBill, BillToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<HouseBillToCreate, T> create, Func<HouseBillToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<HouseBillToCreate> create, Action<HouseBillToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record HouseBillToUpdate : HouseBill, BillToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<HouseBillToCreate, T> create, Func<HouseBillToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<HouseBillToCreate> create, Action<HouseBillToUpdate> update)
        {
            update(this);
        }
    }
}
