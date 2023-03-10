namespace PoundPupLegacy.ViewModel;

public record InterOrganizationalRelation
{
    public required Link OrganizationFrom { get; init; }
    public required Link OrganizationTo { get; init; }
    public required Link InterOrganizationalRelationType { get; init; }
    public Link? GeographicEntity { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public decimal? MoneyInvolved { get; init; }
    public required int NumberOfChildrenInvolved { get; init; }
    public string? Description { get; init; }
    public required int Direction { get; init; }
}
