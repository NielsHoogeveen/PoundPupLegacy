namespace PoundPupLegacy.ViewModel;

public record File
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required long Size { get; init; }
    public required string MimeType { get; init; }
    public string Path { get; init; }
}
