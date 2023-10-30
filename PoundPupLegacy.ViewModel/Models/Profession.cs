namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Profession))]
public partial class ProfessionJsonContext : JsonSerializerContext { }

public sealed record Profession: NameableBase
{
}
