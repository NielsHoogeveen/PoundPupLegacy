namespace PoundPupLegacy.CreateModel;
public sealed record RepresentativeHouseBillAction: NodeToCreate
{
    public required RepresentativeHouseBillActionDetails RepresentativeHouseBillActionDetails { get; init; }
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public NodeDetails NodeDetails => NodeDetailsForCreate;
}


public sealed record RepresentativeHouseBillActionDetails
{
    public required int RepresentativeId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }

}
