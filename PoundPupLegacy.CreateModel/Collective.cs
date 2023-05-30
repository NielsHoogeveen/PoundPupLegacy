namespace PoundPupLegacy.CreateModel;

public sealed record Collective : Publisher
{
    public required Identification.Possible Identification { get; init; }
    public required string Name { get; init; }
}
