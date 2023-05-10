namespace PoundPupLegacy.EditModel;

public record InterOrganizationalRelationTypeListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
