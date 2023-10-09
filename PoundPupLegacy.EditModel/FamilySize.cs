namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(FamilySize))]
public partial class FamilySizeJsonContext : JsonSerializerContext { }

public sealed record FamilySize: ListEditElement
{
}
