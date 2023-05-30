namespace PoundPupLegacy.CreateModel;

public sealed record BasicAction : Action
{
    public required Identification.Possible IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public required string Path { get; init; }
    public required string Description { get; init; }
}

