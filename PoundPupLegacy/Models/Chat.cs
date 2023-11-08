namespace PoundPupLegacy.Models;

public enum ActivationStatus
{
    Closed,
    Open,
    OpenCollaped
}

public record Chat
{
    public required int Id { get; init; }
    public required string? Name { get; init; }
    public string? Description => Name ?? Participants.Where(x => !x.IsCurrentUser).Select(x => x.Name).First();

    private List<ChatMessage> _messages = new();
    public required List<ChatMessage> Messages {
        get => _messages;
        init { 
            if(value is not null) {
                _messages = value;
            }
        }
    }
    public required List<ChatParticipant> Participants { get; init; }
    public bool HasUnreadMessages  {
        get{
            if(LatestMessage is null) {              
                return false;
            }
            if(Self.TimestampLastRead is null) {
                return true;
            }
            return Self.TimestampLastRead < LatestMessage.Timestamp;
        }
    }
    public ChatParticipant Self => Participants.First(x => x.IsCurrentUser);
    public ChatMessage? LatestMessage => Messages.OrderBy(x => x.Timestamp).LastOrDefault();
}
