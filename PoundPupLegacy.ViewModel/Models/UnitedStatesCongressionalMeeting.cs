namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesCongressionalMeeting))]
public partial class UnitedStatesCongressionalMeetingJsonContext : JsonSerializerContext { }

public sealed record UnitedStatesCongressionalMeeting : NameableBase
{
    public required DateTime DateFrom { get; init; }

    public required DateTime DateTo { get; init; }

    public required int Number { get; init; }
}

