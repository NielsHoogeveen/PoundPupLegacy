namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonListItem))]
public partial class PersonListItemJsonContext : JsonSerializerContext { }

public record PersonListItem : PartyListItem
{
}
