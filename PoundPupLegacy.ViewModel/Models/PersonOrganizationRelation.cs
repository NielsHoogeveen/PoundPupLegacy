namespace PoundPupLegacy.ViewModel.Models;

public record PersonOrganizationRelation
{
    public required BasicLink Person { get; init; }
    public required string RelationTypeName { get; init; }

    public DateTime? DateFrom { get; init; }

    public DateTime? DateTo { get; init; }
}
