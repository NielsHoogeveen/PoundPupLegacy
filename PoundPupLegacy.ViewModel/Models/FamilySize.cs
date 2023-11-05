namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FamilySize))]
public partial class FamilySizeJsonContext : JsonSerializerContext { }

public sealed record FamilySize : NameableBase
{
}
