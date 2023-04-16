namespace PoundPupLegacy.ViewModel.Models;

public record OrganizationPersonRelation
{
    public required Link Organization { get; init; }
    public required string RelationTypeName { get; init; }

    public DateTime? DateFrom { get; init; }

    public DateTime? DateTo { get; init; }
}
