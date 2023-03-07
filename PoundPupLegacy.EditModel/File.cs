namespace PoundPupLegacy.EditModel;

public record File
{
    public required int? Id { get; init; }
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required long Size { get; init; }
    public required string MimeType { get; init; }
    public required bool HasBeenStored { get; init; }
    public bool HasBeenDeleted { get; set; } = false;
}
