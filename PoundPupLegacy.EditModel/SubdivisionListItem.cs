namespace PoundPupLegacy.EditModel;

public record SubdivisionListItem : EditListItem
{
    public required int? Id { get; init; }

    public required string Name { get; init; }
}
