namespace PoundPupLegacy.CreateModel;

public sealed record LocationLocatable : IRequest
{
    public required int LocatableId { get; init; }

    public required int LocationId { get; set; }
}
