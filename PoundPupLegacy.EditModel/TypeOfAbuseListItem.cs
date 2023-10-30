namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuseListItem))]
public partial class TypeOfAbuseListItemJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuseListItem: ListEditElement
{

}
