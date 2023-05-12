namespace PoundPupLegacy.EditModel;

public record PersonListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; set; }
}
