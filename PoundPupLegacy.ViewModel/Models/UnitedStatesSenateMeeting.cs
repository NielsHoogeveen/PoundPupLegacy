namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(UnitedStatesSenateMeeting))]
public partial class UnitedStatesSenateMeetingJsonContext : JsonSerializerContext { }

public sealed record UnitedStatesSenateMeeting
{
    public required string Name { get; init; }
    public required DateTime From { get; init; }
    public required DateTime To { get; init; }

    private StateRepresentation[] states = Array.Empty<StateRepresentation>();
    public required StateRepresentation[] States {
        get => states;
        init {
            if (value is not null) {
                states = value;
            }
        }
    }
}

