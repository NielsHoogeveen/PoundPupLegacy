namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuse))]
public partial class TypeOfAbuseJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuse
{
    public int Id { get; init; }

    public required string Name { get; init; }

}
