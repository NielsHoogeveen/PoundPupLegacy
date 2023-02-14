namespace PoundPupLegacy.ViewModel;

public record PersonOrganizationRelation
{
    public required Link Person { get; init; }

    public required string RelationTypeName { get; init; }

    public required DateTime? DateFrom { get; init; }

    public required DateTime? DateTo { get; init; }
}
