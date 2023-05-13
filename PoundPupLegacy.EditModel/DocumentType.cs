namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DocumentType))]
public partial class DocumentTypeJsonContext : JsonSerializerContext { }

public record DocumentType
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsSelected { get; set; }
}
