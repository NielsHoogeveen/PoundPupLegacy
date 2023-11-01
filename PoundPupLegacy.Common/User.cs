namespace PoundPupLegacy.Common;

public record User
{
    public required int Id { get; init; }
    public required string? Name { get; init; }

}

