namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesCounty))]
public partial class UnitedStatesCountyJsonContext : JsonSerializerContext { }

public sealed record UnitedStatesCounty : NameableBase
{
    public required LinkBase State { get; init; }
    public required int Fips { get; init; }
}
