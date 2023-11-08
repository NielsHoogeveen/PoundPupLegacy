namespace PoundPupLegacy.Models;

public record ChatParticipant
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateTime? TimestampLastRead { get; set; }
    public required bool IsCurrentUser { get; init; }
}
