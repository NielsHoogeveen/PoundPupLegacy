namespace PoundPupLegacy.ViewModel.Models;

public abstract record TopLevelCountryBase: CountryBase, TopLevelCountry
{
    public required string ISO3166_1_Code { get; init; }
    public required BasicLink GlobalRegion { get; init; }

}

public interface TopLevelCountry : Country
{
    string ISO3166_1_Code { get; }

    BasicLink GlobalRegion { get; }

}
