namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Image))]
public partial class ImageJsonContext : JsonSerializerContext { }


public sealed record Image
{
    public required string FilePath { get; init; }

    public required string? Label { get; init; }
}
