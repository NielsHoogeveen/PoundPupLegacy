namespace PoundPupLegacy.CreateModel;

public record EditNodeAction : Action
{
    public required int? Id { get; set; }

    public required int NodeTypeId { get; init; }
}
