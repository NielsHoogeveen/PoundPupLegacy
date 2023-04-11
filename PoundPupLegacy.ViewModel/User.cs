namespace PoundPupLegacy.ViewModel;

public record User
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required DateTime Created { get; init; }
    public required string? AboutMe { get; init; }
    public required string? AnimalWithin { get; init; }
    public required string RelationToChildPlacement { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string? Avatar { get; init; }
}