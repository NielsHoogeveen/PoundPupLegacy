namespace PoundPupLegacy.Model;

public sealed record LocationLocatable
{
    public required int LocationId { get; init; }
    public required int LocatableId { get; init; }
}
