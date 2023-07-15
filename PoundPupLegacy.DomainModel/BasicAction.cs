namespace PoundPupLegacy.DomainModel;

public sealed record BasicAction : Action
{
    public required Identification.Possible Identification { get; init; }
    public required string Path { get; init; }
    public required string Description { get; init; }
}

