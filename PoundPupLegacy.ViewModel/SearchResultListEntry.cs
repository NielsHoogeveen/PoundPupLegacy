namespace PoundPupLegacy.ViewModel;

public record SearchResultListEntry : ListEntry
{
    public string Path { get; set; }

    public string Title { get; set; }

    public string Teaser { get; set; }

    public string NodeTypeName { get; set; }

    public int Status { get; set; }

}
