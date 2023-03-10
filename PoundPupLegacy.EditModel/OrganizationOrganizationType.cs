namespace PoundPupLegacy.EditModel;

public record OrganizationOrganizationType
{
    public int? OrganizationId { get; set; }

    public int OrganizationTypeId { get; init; }

    public string Name { get; init; }
    public bool HasBeenStored { get; init; }
    public bool HasBeenDeleted { get; init; }
}
