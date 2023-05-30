namespace PoundPupLegacy.CreateModel;

public abstract record SenateBill : Bill
{
    private SenateBill() { }
    public required BillDetails BillDetails { get; init; }
    public sealed record ToCreate : SenateBill, BillToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetails { get; init; }
        public required NameableDetails.ForCreate NameableDetails { get; init; }
    }
    public sealed record ToUpdate : SenateBill, BillToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required NodeDetails.ForUpdate NodeDetails { get; init; }
        public required NameableDetails.ForUpdate NameableDetails { get; init; }
    }
}
