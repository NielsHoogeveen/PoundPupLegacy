namespace PoundPupLegacy.CreateModel;

public sealed record LocationLocatable : IRequest
{
    public required int? LocationId { get; set; }
    public required int LocatableId { get; init; }
}
