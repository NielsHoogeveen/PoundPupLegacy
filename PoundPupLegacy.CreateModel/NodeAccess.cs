namespace PoundPupLegacy.DomainModel;

public record NodeAccess : PossiblyIdentifiable
{
    public required Identification.Possible Identification { get; init; }
    public required DateTime DateTime { get; init; }
    public required int UserId { get; init; }
    public required int TenantId { get; init; }
    public required int NodeId { get; init; }
}
