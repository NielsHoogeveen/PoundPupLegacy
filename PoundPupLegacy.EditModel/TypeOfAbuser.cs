namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuser))]
public partial class TypeOfAbuserJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuser
{
    public int Id { get; init; }

    public required string Name { get; init; }
    public bool HasBeenDeleted { get; set; } = false;

}
