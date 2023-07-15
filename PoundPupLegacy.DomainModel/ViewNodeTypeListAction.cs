namespace PoundPupLegacy.DomainModel;

public sealed record ViewNodeTypeListAction : IRequest
{
    public required int BasicActionId { get; set; }
    public required int NodeTypeId { get; set; }
}