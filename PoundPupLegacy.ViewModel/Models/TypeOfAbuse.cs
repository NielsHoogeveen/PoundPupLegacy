namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TypeOfAbuse))]
public partial class TypeOfAbuseJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuse: NameableBase
{
}
