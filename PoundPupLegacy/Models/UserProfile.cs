using System.Text.Json.Serialization;

namespace PoundPupLegacy.Models;

[JsonSerializable(typeof(UserProfile))]
internal partial class UserProfileJsonContext : JsonSerializerContext { }

public record UserProfile
{
    public required int Id { get; init; }
    public required string Name { get; set; }
    public required string? AboutMe { get; set; }

    public required string? AnimalWithin { get; set; }

    public required string? Avatar { get; set; }

    public required int? RelationToChildPlacementId { get; set; }

    public required RelationToChildPlacement[] RelationsToChildPlacement { get; set; }
}

public record RelationToChildPlacement
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}