namespace PoundPupLegacy.CreateModel;
public sealed record SenatorSenateBillAction : NodeToCreate
{
    public required SenatorSenateBillActionDetails SenatorSenateBillActionDetails { get; init; }
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public NodeDetails NodeDetails => NodeDetailsForCreate;
}


public sealed record SenatorSenateBillActionDetails
{
    public required int SenatorId { get; init; }
    public required int SenateBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }

}
