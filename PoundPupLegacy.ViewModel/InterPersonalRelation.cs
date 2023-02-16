namespace PoundPupLegacy.ViewModel;

public record InterPersonalRelation
{
    public required Link PersonFrom { get; init; }
    public required Link PersonTo { get; init; }
    public required Link InterPersonalRelationType { get; init; }
    public required DateTime? DateFrom { get; init; }
    public required DateTime? DateTo { get; init; }
    public required int Direction { get; init; }
}
