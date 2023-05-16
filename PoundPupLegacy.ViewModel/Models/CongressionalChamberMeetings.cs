namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CongressionalChamberMeetings))]
public partial class CongressionalChamberMeetingsJsonContext : JsonSerializerContext { }

public sealed record CongressionalChamberMeetings : Link
{
    public required string Title { get; init; }

    public required string Path { get; init; }

    public required DateTime DateFrom { get; init; }

    public required DateTime DateTo { get; init; }
}
