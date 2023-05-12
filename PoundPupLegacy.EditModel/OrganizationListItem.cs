namespace PoundPupLegacy.EditModel;

public record OrganizationListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; set; }
}
