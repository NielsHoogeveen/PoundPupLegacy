namespace PoundPupLegacy.ViewModel;

public record SecondLevelRegionListEntry
{
    public string Name { get; set; }
    public string Path { get; set; }
    public CountryListEntry[] Countries { get; set; }
}

