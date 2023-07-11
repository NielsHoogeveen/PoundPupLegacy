namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(DocumentListItem))]
public partial class DocumentListItemJsonContext : JsonSerializerContext { }

public sealed record DocumentListItem : EditListItemBase<DocumentListItem>
{
}
