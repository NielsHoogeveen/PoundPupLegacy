namespace PoundPupLegacy.ViewModel;

public interface TopLevelCountry : Country
{
    public string ISO3166_1_Code { get; }

    Link GlobalRegion { get; }
}
