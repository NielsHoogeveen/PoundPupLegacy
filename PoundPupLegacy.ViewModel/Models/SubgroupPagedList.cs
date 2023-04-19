namespace PoundPupLegacy.ViewModel.Models;

public record SubgroupPagedList : IPagedList<SubgroupListEntry>
{
    public required SubgroupListEntry[] Entries { get; init; }

    public int NumberOfEntries { get; set; }
}
