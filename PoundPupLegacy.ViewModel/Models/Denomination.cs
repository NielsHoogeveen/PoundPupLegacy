namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Denomination))]
public partial class DenominationJsonContext : JsonSerializerContext { }

public sealed record Denomination: NameableBase
{
}
