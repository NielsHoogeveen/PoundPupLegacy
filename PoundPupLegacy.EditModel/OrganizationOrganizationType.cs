namespace PoundPupLegacy.EditModel;

public record OrganizationOrganizationType
{
    public int? OrganizationId { get; set; }

    public required int OrganizationTypeId { get; init; }

    public required string Name { get; init; }
    public required bool HasBeenStored { get; init; }
    public required bool HasBeenDeleted { get; init; }
}
