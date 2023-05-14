namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PartyListItem))]
public partial class PartyListItemJsonContext : JsonSerializerContext { }


public interface PartyItem: Named
{

}
public record PartyListItem : EditListItem, PartyItem
{
    public required int? Id { get; init; }
    public required string Name { get; set; }
}
public record PartyName : PartyItem
{
    public required string Name { get; set; }
}

