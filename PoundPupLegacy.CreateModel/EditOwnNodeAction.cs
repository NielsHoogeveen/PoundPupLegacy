namespace PoundPupLegacy.CreateModel;

public sealed record EditOwnNodeAction : Action
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public required int NodeTypeId { get; init; }
}
