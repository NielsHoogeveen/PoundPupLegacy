namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(InformalSubdivision))]
public partial class InformalSubdivisionJsonContext : JsonSerializerContext { }

public sealed record InformalSubdivision : SubdivisionBase
{
}
