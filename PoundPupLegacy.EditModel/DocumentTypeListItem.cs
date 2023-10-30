namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DocumentTypeListItem))]
public partial class DocumentTypeListItemJsonContext : JsonSerializerContext { }

public sealed record DocumentTypeListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required bool IsSelected { get; set; }
}
