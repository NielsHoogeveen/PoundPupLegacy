namespace PoundPupLegacy.EditModel;

public record PoliticalEntityListItem : EditListItem
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
