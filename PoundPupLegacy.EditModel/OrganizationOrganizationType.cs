namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(OrganizationOrganizationType))]
public partial class OrganizationOrganizationTypeJsonContext : JsonSerializerContext { }

public sealed record OrganizationOrganizationType
{
    public int? OrganizationId { get; set; }

    public required int OrganizationTypeId { get; init; }

    public required string Name { get; init; }
    public required bool HasBeenStored { get; init; }
    public required bool HasBeenDeleted { get; set; }
}
