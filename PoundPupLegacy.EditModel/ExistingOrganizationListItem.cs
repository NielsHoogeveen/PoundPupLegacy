namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingOrganizationListItem))]
public partial class ExistingOrganizationListItemJsonContext : JsonSerializerContext { }


public interface _ListItem
{
    string Name { get; }
}
public interface _PartyListItem: _ListItem
{

}
public interface _OrganizationListItem: _PartyListItem
{
}
public interface _PersonListItem : _PartyListItem
{
}

public interface _ExistingListItem
{
    int Id { get; }
}
public interface _NewListItem
{
}

public record ExistingOrganizationListItem : _OrganizationListItem, _ExistingListItem
{
    public required int Id { get; init; }
    public required string Name { get; set; }
}
public record NewOrganizationListItem : _OrganizationListItem, _NewListItem
{
    public required string Name { get; set; }
}

public record ExistingPersonListItem : _PersonListItem, _ExistingListItem
{
    public required int Id { get; init; }
    public required string Name { get; set; }
}
public record NewPersonListItem : _PersonListItem, _NewListItem
{
    public required string Name { get; set; }
}