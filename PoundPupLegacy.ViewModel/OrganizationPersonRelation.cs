namespace PoundPupLegacy.ViewModel;

public record OrganizationPersonRelation
{
    public required Link Organization { get; init; }
    public required string RelationTypeName { get; init; }

    public required DateTime? DateFrom { get; init; }

    public required DateTime? DateTo { get; init; }
}
