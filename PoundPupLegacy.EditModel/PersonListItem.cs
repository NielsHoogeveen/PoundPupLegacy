namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonListItem))]
public partial class PersonListItemJsonContext : JsonSerializerContext { }


public interface PersonItem : PartyItem
{
}
public record PersonListItem : PartyListItem, PersonItem
{
}
public record PersonName: PartyName, PersonItem
{
}
