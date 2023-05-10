namespace PoundPupLegacy.EditModel;

public record GeographicalEntityListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
