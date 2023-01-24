namespace PoundPupLegacy.ViewModel;

public record DocumentListItem
{
    public string Path { get; set; }

    public string Title { get; set; }

    public string PublicationDate { get; set; }

    public int SortOrder { get; set; }
}
