namespace PoundPupLegacy.ViewModel.Models;

public record SecondLevelRegionListEntry
{
    public required string Name { get; init; }
    public required string Path { get; init; }
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

