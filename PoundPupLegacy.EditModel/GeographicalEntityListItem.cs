namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(GeographicalEntityListItem))]
public partial class GeographicalEntityListItemJsonContext : JsonSerializerContext { }

public sealed record GeographicalEntityListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
