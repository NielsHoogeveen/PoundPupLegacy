namespace PoundPupLegacy.ViewModel.Models;

public record Persons : PagedList<PersonListEntry>
{
    private PersonListEntry[] _entries = Array.Empty<PersonListEntry>();
    public required PersonListEntry[] Entries
    {
        get => _entries;
        init
        {
            if (value is not null)
            {
                _entries = value;
            }
        }
    }

    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";
    public SearchOption SelectedSearchOption { get; set; }
    public string? SearchTerm { get; set; }
    public string Path => "Persons";

}
