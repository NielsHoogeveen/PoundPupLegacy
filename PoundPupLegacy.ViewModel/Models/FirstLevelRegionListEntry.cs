namespace PoundPupLegacy.ViewModel.Models;

public record FirstLevelRegionListEntry
{
    public required string Name { get; init; }
    public required string Path { get; init; }

    private SecondLevelRegionListEntry[] _regions = Array.Empty<SecondLevelRegionListEntry>();
    public required SecondLevelRegionListEntry[] Regions
    {
        get => _regions;
        init
        {
            if (value is not null)
                _regions = value;
        }
    }
    private CountryListEntry[] _countries = Array.Empty<CountryListEntry>();
    public required CountryListEntry[] Countries
    {
        get => _countries;
        init
        {
            if (value is not null)
                _countries = value;
        }
    }
}

