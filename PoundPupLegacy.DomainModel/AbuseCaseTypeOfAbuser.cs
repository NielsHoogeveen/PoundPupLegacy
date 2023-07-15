namespace PoundPupLegacy.DomainModel;

public sealed record AbuseCaseTypeOfAbuser : IRequest
{
    public required int AbuseCaseId { get; set; }

    public required int TypeOfAbuserId { get; set; }

}
