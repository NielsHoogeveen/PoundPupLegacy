namespace PoundPupLegacy.ViewModel.Models;

public record SubgroupPagedList 
{
    public required SubgroupListEntry[] Entries { get; init; }

    public int NumberOfEntries { get; set; }
}
