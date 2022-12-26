namespace PoundPupLegacy.Model;

public record LocationLocatable
{
    public required int LocationId { get; init; }
    public required int LocatableId { get; init; }
}
