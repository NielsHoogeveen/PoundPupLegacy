namespace PoundPupLegacy.DomainModel;

public sealed record Subgroup : PublishingUserGroup
{
    public required Identification.Possible Identification { get; init; }
    public required int PublicationStatusIdDefault { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int TenantId { get; init; }
    public required AdministratorRole AdministratorRole { get; init; }

}
