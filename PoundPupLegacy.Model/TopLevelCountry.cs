namespace PoundPupLegacy.CreateModel;

public interface TopLevelCountry : Country
{
    public string ISO3166_1_Code { get; }

    public int SecondLevelRegionId { get; }
}
