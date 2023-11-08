namespace PoundPupLegacy.Models;

public record ChatMessage
{
    public required int Id { get; init; }
    public required int ParticipantId { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string Text { get; init; }
}
