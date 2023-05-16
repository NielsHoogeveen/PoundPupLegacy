namespace PoundPupLegacy.CreateModel;

public sealed record EditOwnNodeAction : Action
{
    public required int? Id { get; set; }

    public required int NodeTypeId { get; init; }
}
