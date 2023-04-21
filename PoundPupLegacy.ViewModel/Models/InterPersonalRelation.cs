namespace PoundPupLegacy.ViewModel.Models;

public record InterPersonalRelation
{
    public required BasicLink PersonFrom { get; init; }
    public required BasicLink PersonTo { get; init; }
    public required BasicLink InterPersonalRelationType { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public required int Direction { get; init; }
}
