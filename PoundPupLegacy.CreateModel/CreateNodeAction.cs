namespace PoundPupLegacy.DomainModel;

public sealed record CreateNodeAction : Action
{
    public required Identification.Possible Identification { get; init; }
    public required int NodeTypeId { get; init; }
}
