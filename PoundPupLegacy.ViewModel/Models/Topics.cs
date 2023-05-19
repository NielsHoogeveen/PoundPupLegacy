namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Topics))]
public partial class TopicsJsonContext : JsonSerializerContext { }

public sealed record Topics : PagedListBase<TopicListEntry>
{

}
