namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(HagueStatus))]
public partial class HagueStatusJsonContext : JsonSerializerContext { }

public sealed record HagueStatus: NameableBase
{
}
