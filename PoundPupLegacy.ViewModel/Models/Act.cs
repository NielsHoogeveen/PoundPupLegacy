namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Act))]
public partial class ActJsonContext : JsonSerializerContext { }

public sealed record Act : NameableBase
{
}
