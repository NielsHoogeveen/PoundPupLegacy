namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(SubgroupPagedList))]
public partial class SubgroupPagedListJsonContext : JsonSerializerContext { }

public sealed record SubgroupPagedList : PagedListBase<SubgroupListEntry>
{
}
