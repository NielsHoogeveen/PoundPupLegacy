namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SecondLevelRegionListEntry))]
public partial class SecondLevelRegionListEntryJsonContext : JsonSerializerContext { }

public sealed record SecondLevelRegionListEntry : ListEntryBase
{
    private CountryListEntry[] _countries = Array.Empty<CountryListEntry>();
    public required CountryListEntry[] Countries {
        get => _countries;
        init {
            if (value is not null)
                _countries = value;
        }
    }
}

