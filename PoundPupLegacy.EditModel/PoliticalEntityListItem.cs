namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PoliticalEntityListItem))]
public partial class PoliticalEntityListItemJsonContext : JsonSerializerContext { }

public sealed record PoliticalEntityListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
