namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(SubdivisionListItem))]
public partial class SubdivisionListItemJsonContext : JsonSerializerContext { }

public sealed record SubdivisionListItem : EditListItemBase<SubdivisionListItem>
{
}
