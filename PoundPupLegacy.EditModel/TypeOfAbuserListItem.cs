namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuserListItem))]
public partial class TypeOfAbuserListItemJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuserListItem: ListEditElement
{
}
