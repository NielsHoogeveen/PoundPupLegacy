namespace PoundPupLegacy.CreateModel;

public sealed record EditNodeAction : Action
{
    public required Identification.Possible Identification { get; init; }
    public required int NodeTypeId { get; init; }
}
