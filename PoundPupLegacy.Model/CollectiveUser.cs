namespace PoundPupLegacy.Model;

public record CollectiveUser
{
    public required int? CollectiveId { get; set; }
    public required int? UserId { get; set; }
}
