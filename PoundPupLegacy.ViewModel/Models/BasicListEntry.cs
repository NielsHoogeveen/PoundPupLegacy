namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicListEntry))]
public partial class BasicListEntryJsonContext : JsonSerializerContext { }

public sealed record BasicListEntry : ListEntryBase
{
}
