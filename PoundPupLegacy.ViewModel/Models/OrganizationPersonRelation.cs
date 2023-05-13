namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(OrganizationPersonRelation))]
public partial class OrganizationPersonRelationJsonContext : JsonSerializerContext { }

public record OrganizationPersonRelation
{
    public required BasicLink Organization { get; init; }
    public required string RelationTypeName { get; init; }

    public DateTime? DateFrom { get; init; }

    public DateTime? DateTo { get; init; }
}
