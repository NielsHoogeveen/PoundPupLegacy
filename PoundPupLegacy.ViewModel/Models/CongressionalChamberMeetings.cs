namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CongressionalChamberMeetings))]
public partial class CongressionalChamberMeetingsJsonContext : JsonSerializerContext { }

public sealed record CongressionalChamberMeetings : LinkBase
{
    public required DateTime DateFrom { get; init; }

    public required DateTime DateTo { get; init; }
}
