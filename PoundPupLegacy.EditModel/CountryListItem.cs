namespace PoundPupLegacy.EditModel;

public record CountryListItem: EditListItem
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}
