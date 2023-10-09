namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuse))]
public partial class TypeOfAbuseJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuse: ListEditElement
{

}
