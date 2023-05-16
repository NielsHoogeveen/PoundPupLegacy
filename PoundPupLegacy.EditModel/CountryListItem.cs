namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(CountryListItem))]
public partial class CountryListItemJsonContext : JsonSerializerContext { }

public sealed record CountryListItem : EditListItem
{
    public required int Id { get; init; }

    public required string Name { get; init; }
}
