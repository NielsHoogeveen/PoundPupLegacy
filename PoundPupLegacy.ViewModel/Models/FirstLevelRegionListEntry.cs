namespace PoundPupLegacy.ViewModel.Models;



[JsonSerializable(typeof(FirstLevelRegionListEntry))]
[JsonSerializable(typeof(FirstLevelRegionListEntry[]))]
public partial class FirstLevelRegionListEntryJsonContext : JsonSerializerContext { }

public sealed record FirstLevelRegionListEntry : ListEntryBase
{

    private SecondLevelRegionListEntry[] _regions = Array.Empty<SecondLevelRegionListEntry>();
    public required SecondLevelRegionListEntry[] Regions {
        get => _regions;
        init {
            if (value is not null)
                _regions = value;
        }
    }
    private CountryListEntry[] _countries = Array.Empty<CountryListEntry>();
    public required CountryListEntry[] Countries {
        get => _countries;
        init {
            if (value is not null)
                _countries = value;
        }
    }
}

