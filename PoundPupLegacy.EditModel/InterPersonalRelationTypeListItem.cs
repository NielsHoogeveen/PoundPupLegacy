namespace PoundPupLegacy.EditModel;

public record InterPersonalRelationTypeListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
