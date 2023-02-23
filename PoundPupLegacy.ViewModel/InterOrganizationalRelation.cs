namespace PoundPupLegacy.ViewModel;

public record InterOrganizationalRelation
{
    public required Link OrganizationFrom { get; init; }
    public required Link OrganizationTo { get; init; }
    public required Link InterOrganizationalRelationType { get; init; }
    public required Link? GeographicEntity { get; init; }
    public required DateTime? DateFrom { get; init; }
    public required DateTime? DateTo { get; init; }
    public required decimal? MoneyInvolved { get; init; }
    public required int NumberOfChildrenInvolved { get; init; }
    public required string? Description { get; init; }
    public required int Direction { get; init; }
}
