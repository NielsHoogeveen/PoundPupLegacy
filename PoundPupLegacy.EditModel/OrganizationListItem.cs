namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationListItem))]
public partial class OrganizationListItemJsonContext : JsonSerializerContext { }

public interface OrganizationItem : PartyItem
{
}
public record OrganizationListItem : PartyListItem, OrganizationItem
{
}

public record OrganizationName: PartyName, OrganizationItem
{
}
