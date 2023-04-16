namespace PoundPupLegacy.ViewModel.Models;
public record UnitedStatesCongress
{
    public required CongressionalChamber Senate { get; init; }

    public required CongressionalChamber House { get; init; }

}

