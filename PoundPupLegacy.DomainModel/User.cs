namespace PoundPupLegacy.DomainModel;

public abstract record User : Publisher
{
    public required string Name { get; init; }
    public required DateTime CreatedDateTime { get; init; }
    public required string? AboutMe { get; init; }
    public required string? AnimalWithin { get; init; }
    public required string RelationToChildPlacement { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string? Avatar { get; init; }
    public required int UserStatusId { get; init; }
    public required DateTime? ExpiryDateTime { get; init; }

    public sealed record ToCreate : User, PublisherToCreate
    {
        public required Identification.Possible Identification { get; init; }
        public required List<int> UserRoleIds { get; init; }
    }
    public sealed record ToUpdate : User, PublisherToUpdate
    {
        public required Identification.Certain Identification { get; init; }
        public required List<int> UserRoleIdsToAdd { get; init; }
        public required List<int> UserRoleIdsToRemove { get; init; }
    }
}

