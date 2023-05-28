namespace PoundPupLegacy.CreateModel;

public abstract record SenateBill : Bill
{
    private SenateBill() { }
    public required BillDetails BillDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract NameableDetails NameableDetails { get; }
    public abstract T Match<T>(Func<SenateBillToCreate, T> create, Func<SenateBillToUpdate, T> update);
    public abstract void Match(Action<SenateBillToCreate> create, Action<SenateBillToUpdate> update);

    public sealed record SenateBillToCreate : SenateBill, BillToCreate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override NameableDetails NameableDetails => NameableDetailsForCreate;
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public required NameableDetails.NameableDetailsForCreate NameableDetailsForCreate { get; init; }
        public override T Match<T>(Func<SenateBillToCreate, T> create, Func<SenateBillToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<SenateBillToCreate> create, Action<SenateBillToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record SenateBillToUpdate : SenateBill, BillToUpdate
    {
        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override NameableDetails NameableDetails => NameableDetailsForUpdate;
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NameableDetails.NameableDetailsForUpdate NameableDetailsForUpdate { get; init; }
        public override T Match<T>(Func<SenateBillToCreate, T> create, Func<SenateBillToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<SenateBillToCreate> create, Action<SenateBillToUpdate> update)
        {
            update(this);
        }
    }
}
