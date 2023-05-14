namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationListItem))]
public partial class OrganizationListItemJsonContext : JsonSerializerContext { }

public record OrganizationListItem : PartyListItem, OrganizationItem
{
}

public record OrganizationName: OrganizationItem
{
    public required string Name { get; init; }
}
public interface OrganizationItem: Named
{
}