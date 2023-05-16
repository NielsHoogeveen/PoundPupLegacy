namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(SubdivisionListItem))]
public partial class SubdivisionListItemJsonContext : JsonSerializerContext { }

public sealed record SubdivisionListItem : EditListItem
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}
