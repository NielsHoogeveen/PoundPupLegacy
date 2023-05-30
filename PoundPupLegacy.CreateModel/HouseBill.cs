namespace PoundPupLegacy.CreateModel;

public abstract record HouseBill : Bill
{
    private HouseBill() { }
    public required BillDetails BillDetails { get; init; }
    public sealed record ToCreate : HouseBill, BillToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.ForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record HouseBillToUpdate : HouseBill, BillToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
