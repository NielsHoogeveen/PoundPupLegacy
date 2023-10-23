namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TopicListEntry))]
public partial class TopicListEntryJsonContext : JsonSerializerContext { }

public sealed record TopicListEntry : EntityListEntryBase
{


}
