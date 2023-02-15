namespace PoundPupLegacy.ViewModel;

public record File
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required int Size { get; init; }
    public required string MimeType { get; init; }
}
