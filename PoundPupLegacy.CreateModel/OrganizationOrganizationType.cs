namespace PoundPupLegacy.CreateModel;

public record class OrganizationOrganizationType
{
    public required int? OrganizationId { get; set; }

    public required int OrganizationTypeId { get; init; }
}
