namespace PoundPupLegacy.CreateModel;

public sealed record Subgroup : PublishingUserGroup
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public required int PublicationStatusIdDefault { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int TenantId { get; init; }
    public required AdministratorRole AdministratorRole { get; init; }

}
