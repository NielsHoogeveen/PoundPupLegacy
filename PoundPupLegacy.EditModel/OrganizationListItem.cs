namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationListItem))]
public partial class OrganizationListItemJsonContext : JsonSerializerContext { }

public record OrganizationListItem : PartyListItem
{
}