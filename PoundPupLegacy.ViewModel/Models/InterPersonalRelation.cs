namespace PoundPupLegacy.ViewModel.Models;

public record InterPersonalRelation
{
    public required Link PersonFrom { get; init; }
    public required Link PersonTo { get; init; }
    public required Link InterPersonalRelationType { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public required int Direction { get; init; }
}
