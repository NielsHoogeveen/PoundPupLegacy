namespace PoundPupLegacy.ViewModel.Models;

public interface TopLevelCountry : Country
{
    string ISO3166_1_Code { get; }

    BasicLink GlobalRegion { get; }

}
