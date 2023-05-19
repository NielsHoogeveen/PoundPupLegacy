namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Polls))]
public partial class PollsJsonContext : JsonSerializerContext { }

public sealed record Polls : PagedListBase<PollListEntry>
{
}
