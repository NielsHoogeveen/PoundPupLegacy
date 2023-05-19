namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Discussion))]
public partial class DiscussionJsonContext : JsonSerializerContext { }

public sealed record Discussion : SimpleTextNodeBase
{
}
