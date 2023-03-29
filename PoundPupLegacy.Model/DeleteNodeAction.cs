namespace PoundPupLegacy.CreateModel;

public record DeleteNodeAction : Action
{
    public required int? Id { get; set; }

    public required int NodeTypeId { get; init; }
}
