namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubgroupPagedList))]
public partial class SubgroupPagedListJsonContext : JsonSerializerContext { }

public record SubgroupPagedList : IPagedList<SubgroupListEntry>
{
    public required SubgroupListEntry[] Entries { get; init; }

    public int NumberOfEntries { get; set; }
}
