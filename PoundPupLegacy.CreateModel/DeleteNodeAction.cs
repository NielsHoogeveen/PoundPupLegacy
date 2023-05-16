namespace PoundPupLegacy.CreateModel;

public sealed record DeleteNodeAction : Action
{
    public required int? Id { get; set; }

    public required int NodeTypeId { get; init; }
}
