namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Document))]
public partial class DocumentJsonContext : JsonSerializerContext { }

public sealed record Document : SimpleTextNodeBase
{
    public required FuzzyDate? Published { get; init; }
    public BasicLink? DocumentType { get; init; }
    public string? SourceUrl { get; init; }
    public string? SourceUrlHost => SourceUrl is null ? null : new Uri(SourceUrl).Host;
}
