namespace PoundPupLegacy.Model;

public record EditOwnNodeAction : Action
{
    public required int? Id { get; set; }

    public required int NodeTypeId { get; init; }
}
