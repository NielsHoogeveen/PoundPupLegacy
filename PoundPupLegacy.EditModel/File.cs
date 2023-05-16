namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(File))]
public partial class FileJsonContext : JsonSerializerContext { }

public sealed record File
{
    public required int? Id { get; init; }
    public required string Name { get; init; }
    public required string Path { get; init; }
    public required long Size { get; init; }
    public required string MimeType { get; init; }
    public required bool HasBeenStored { get; init; }
    public required int? NodeId { get; set; }
    public bool HasBeenDeleted { get; set; } = false;

}
