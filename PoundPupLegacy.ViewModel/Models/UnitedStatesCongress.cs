namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesCongress))]
public partial class UnitedStatesCongressJsonContext : JsonSerializerContext { }

public record UnitedStatesCongress
{
    public required CongressionalChamber Senate { get; init; }

    public required CongressionalChamber House { get; init; }

}

