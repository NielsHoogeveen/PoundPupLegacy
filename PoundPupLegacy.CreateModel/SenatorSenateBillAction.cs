namespace PoundPupLegacy.DomainModel;
public sealed record SenatorSenateBillAction : NodeToCreate
{
    public required SenatorSenateBillActionDetails SenatorSenateBillActionDetails { get; init; }
    public required Identification.Possible Identification { get; init; }
    public required NodeDetails.ForCreate NodeDetails { get; init; }
}

public sealed record SenatorSenateBillActionDetails
{
    public required int SenatorId { get; init; }
    public required int SenateBillId { get; init; }
    public required int BillActionTypeId { get; init; }
    public required DateTime Date { get; init; }

}
