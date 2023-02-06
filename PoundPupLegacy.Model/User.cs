namespace PoundPupLegacy.Model;

public sealed record User : Publisher
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required string? AboutMe { get; init; }
    public required string? AnimalWithin { get; init; }
    public required string RelationToChildPlacement { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string? Avatar { get; init; }
    public required int UserStatusId { get; init; }
}
