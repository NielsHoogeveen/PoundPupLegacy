namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(TypeOfAbuser))]
public partial class TypeOfAbuserJsonContext : JsonSerializerContext { }

public sealed record TypeOfAbuser: NameableBase
{
}
