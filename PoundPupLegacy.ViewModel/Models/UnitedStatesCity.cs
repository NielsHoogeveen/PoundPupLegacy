namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesCity))]
public partial class UnitedStatesCityJsonContext : JsonSerializerContext { }

public sealed record UnitedStatesCity: NameableBase 
{ 
    public required LinkBase County { get; init; }
    public required int Population { get; init; }
    public required double Density { get; init; }
    public required decimal Latitude { get; init; }
    public required decimal Longitude { get; init; }
    public required string Timezone { get; init; }
    public required bool Incorporated { get; init; }
    public required bool Military { get; init; }
}
