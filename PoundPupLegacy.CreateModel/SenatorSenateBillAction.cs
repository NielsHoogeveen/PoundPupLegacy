namespace PoundPupLegacy.CreateModel;
public sealed record SenateHouseBillAction : NodeToCreate
{
    public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    public NodeIdentification NodeIdentification => NodeIdentificationForCreate;
    public NodeDetails NodeDetails => NodeDetailsForCreate;
}


public abstract record SenateHouseBillActionDetails
{
    public required int SenateId { get; init; }
    public required int HouseBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }

}
