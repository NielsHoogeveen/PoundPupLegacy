namespace PoundPupLegacy.EditModel;

public record PartyListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; set; }
}
