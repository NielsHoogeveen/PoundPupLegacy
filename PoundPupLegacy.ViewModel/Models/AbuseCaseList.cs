namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(AbuseCaseList))]
public partial class AbuseCaseListJsonContext : JsonSerializerContext { }

public sealed record AbuseCaseList : PagedListBase<CaseTeaserListEntry>
{
}
