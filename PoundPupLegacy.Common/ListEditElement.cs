namespace PoundPupLegacy.Common;

public record ListEditElement
{
    public int Id { get; init; }

    public required string Name { get; init; }
    public bool HasBeenDeleted { get; set; } = false;
    public bool HasBeenSelected { get; set; } = false;
}
