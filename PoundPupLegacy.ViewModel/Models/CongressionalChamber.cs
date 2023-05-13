namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(CongressionalChamber))]
public partial class CongressionalChamberJsonContext : JsonSerializerContext { }

public record CongressionalChamber
{
    public required string ImagePath { get; init; }

    private CongressionalChamberMeetings[] meetings = Array.Empty<CongressionalChamberMeetings>();
    public required CongressionalChamberMeetings[] Meetings {
        get => meetings;
        init {
            if (value is not null) {
                meetings = value;
            }
        }
    }
}
