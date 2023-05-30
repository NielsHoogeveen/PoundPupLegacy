namespace PoundPupLegacy.CreateModel;

public sealed record EditNodeAction : Action
{
    public required Identification.Possible IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;

    public required int NodeTypeId { get; init; }
}
