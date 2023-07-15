namespace PoundPupLegacy.DomainModel;
public sealed record RepresentativeHouseBillAction : NodeToCreate
{
    public required RepresentativeHouseBillActionDetails RepresentativeHouseBillActionDetails { get; init; }
    public required Identification.Possible Identification { get; init; }
    public required NodeDetails.ForCreate NodeDetails { get; init; }
}
public sealed record RepresentativeHouseBillActionDetails
{
    public required int RepresentativeId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }
}
