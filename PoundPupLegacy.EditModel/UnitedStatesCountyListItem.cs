namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(UnitedStatesCountyListItem))]
public partial class UnitedStatesCountyListItemJsonContext : JsonSerializerContext { }

public record UnitedStatesCountyListItem : EditListItemBase<CountryListItem>
{
}
