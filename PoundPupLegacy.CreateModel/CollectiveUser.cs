namespace PoundPupLegacy.CreateModel;

public sealed record CollectiveUser : IRequest
{
    public required int? CollectiveId { get; set; }
    public required int? UserId { get; set; }
}
