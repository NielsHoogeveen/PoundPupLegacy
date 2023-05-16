namespace PoundPupLegacy.CreateModel;

public sealed record BasicAction : Action
{
    public required int? Id { get; set; }

    public required string Path { get; init; }

    public required string Description { get; init; }
}

