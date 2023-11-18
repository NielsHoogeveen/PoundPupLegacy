namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesState))]
public partial class UnitedStatesStateJsonContext : JsonSerializerContext { }


public sealed record UnitedStatesState : FormalSubdivisionBase;
