namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(CountryListItem))]
public partial class CountryListItemJsonContext : JsonSerializerContext { }

public record CountryListItem : EditListItemBase<CountryListItem>
{
}
