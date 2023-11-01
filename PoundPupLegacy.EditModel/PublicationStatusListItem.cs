namespace PoundPupLegacy.EditModel;

public sealed record PublicationStatusListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsDefault { get; init; }
}

