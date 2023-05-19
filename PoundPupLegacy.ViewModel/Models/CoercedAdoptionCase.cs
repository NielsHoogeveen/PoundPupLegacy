namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CoercedAdoptionCase))]
public partial class CoercedAdoptionCaseJsonContext : JsonSerializerContext { }

public sealed record CoercedAdoptionCase : CaseBase
{
}
