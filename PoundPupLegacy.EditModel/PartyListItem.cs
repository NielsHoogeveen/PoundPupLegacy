namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PartyListItem))]
public partial class PartyListItemJsonContext : JsonSerializerContext { }

public record PartyListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; set; }
}
