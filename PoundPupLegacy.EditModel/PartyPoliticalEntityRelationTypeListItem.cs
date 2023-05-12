namespace PoundPupLegacy.EditModel;

public record PartyPoliticalEntityRelationTypeListItem : EditListItem
{
    public required int? Id { get; init; }
    public required string Name { get; init; }
}
