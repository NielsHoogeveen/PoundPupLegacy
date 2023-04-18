namespace PoundPupLegacy.ViewModel.Models;

public record Organizations 
{
    private BasicListEntry[] _entries = Array.Empty<BasicListEntry>();
    public required BasicListEntry[] Entries
    {
        get => _entries;
        set
        {
            if (value is not null)
            {
                _entries = value;
            }
        }
    }

    public required int NumberOfEntries { get; init; }
}
