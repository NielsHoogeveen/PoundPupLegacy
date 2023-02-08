namespace PoundPupLegacy.ViewModel;

public record Organizations: PagedList<OrganizationListEntry>
{
    public OrganizationListEntry[] Entries { get; set; }
    public SelectionItem[] Countries { get; set; }
    public SelectionItem[] OrganizationTypes { get; set; }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; }
    public SearchOption SelectedSearchOption { get; set; }
    public string? SearchTerm { get; set; }
    public string Path => "organizations";

}
