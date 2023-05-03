namespace PoundPupLegacy.CreateModel;

public sealed record AbuseCaseTypeOfAbuse : IRequest
{
    public required int AbuseCaseId { get; set; }

    public required int TypeOfAbuseId { get; set; }

}
