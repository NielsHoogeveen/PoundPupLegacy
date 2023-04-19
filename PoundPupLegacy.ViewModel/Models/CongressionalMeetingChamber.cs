namespace PoundPupLegacy.ViewModel.Models;
public record CongressionalMeetingChamber
{
    public required string MeetingName { get; init; }
    public required DateTime DateFrom { get; init; }
    public required DateTime DateTo { get; init; }

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

