namespace PoundPupLegacy.CreateModel;
public sealed record RepresentativeHouseBillAction: NodeToCreate
{
    public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    public NodeIdentification NodeIdentification => NodeIdentificationForCreate;
    public NodeDetails NodeDetails => NodeDetailsForCreate;
}


public abstract record RepresentativeHouseBillActionDetails
{
    public required int RepresentativeId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }

}
