namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuser))]
public partial class TypeOfAbuserJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuser: ListEditElement
{
}
