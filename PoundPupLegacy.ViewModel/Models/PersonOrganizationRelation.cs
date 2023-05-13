namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(PersonOrganizationRelation))]
public partial class PersonOrganizationRelationJsonContext : JsonSerializerContext { }

public record PersonOrganizationRelation
{
    public required BasicLink Person { get; init; }
    public required string RelationTypeName { get; init; }

    public DateTime? DateFrom { get; init; }

    public DateTime? DateTo { get; init; }
}
