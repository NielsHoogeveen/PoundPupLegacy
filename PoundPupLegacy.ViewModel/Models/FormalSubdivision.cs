namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(FormalSubdivision))]
public partial class FormalSubdivisionJsonContext : JsonSerializerContext { }

public sealed record FormalSubdivision : SubdivisionBase
{
    
    public required string ISO3166_2_Code { get; init; }

}
