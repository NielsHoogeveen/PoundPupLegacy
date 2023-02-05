namespace PoundPupLegacy.ViewModel;

public record FirstLevelRegionListEntry
{
    public string Name { get; set; }
    public string Path { get; set; }
    public SecondLevelRegionListEntry[] Regions { get; set; }
    public CountryListEntry[] Countries { get; set; }
}

