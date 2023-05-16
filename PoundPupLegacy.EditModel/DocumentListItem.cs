namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DocumentListItem))]
public partial class DocumentListItemJsonContext : JsonSerializerContext { }

public sealed record DocumentListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
