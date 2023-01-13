namespace PoundPupLegacy.Model;

public sealed record CollectiveUser
{
    public required int? CollectiveId { get; set; }
    public required int? UserId { get; set; }
}
