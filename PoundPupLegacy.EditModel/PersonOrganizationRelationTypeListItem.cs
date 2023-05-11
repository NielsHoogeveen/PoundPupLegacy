namespace PoundPupLegacy.EditModel;

public record PersonOrganizationRelationTypeListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
