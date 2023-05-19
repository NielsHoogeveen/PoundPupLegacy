namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BasicNameable))]
public partial class BasicNameableJsonContext : JsonSerializerContext { }

public sealed record BasicNameable : NameableBase
{
}
