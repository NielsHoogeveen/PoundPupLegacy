namespace PoundPupLegacy.EditModel;

public record DocumentListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; init; }
}
