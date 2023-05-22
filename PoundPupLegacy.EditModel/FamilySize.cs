namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(FamilySize))]
public partial class FamilySizeJsonContext : JsonSerializerContext { }

public sealed record FamilySize
{
    public int Id { get; init; }

    public required string Name { get; init; }

}
