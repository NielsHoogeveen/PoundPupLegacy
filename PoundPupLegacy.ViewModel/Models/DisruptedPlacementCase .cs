namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DisruptedPlacementCase))]
public partial class DisruptedPlacementCaseJsonContext : JsonSerializerContext { }

public sealed record DisruptedPlacementCase : CaseBase
{
}
