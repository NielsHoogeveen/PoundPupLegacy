namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FormalSubdivision))]
public partial class FormalSubdivisionJsonContext : JsonSerializerContext { }

public sealed record FormalSubdivision : FormalSubdivisionBase;
