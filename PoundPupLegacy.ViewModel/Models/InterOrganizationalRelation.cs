namespace PoundPupLegacy.ViewModel.Models;

public record InterOrganizationalRelation
{
    public required BasicLink OrganizationFrom { get; init; }
    public required BasicLink OrganizationTo { get; init; }
    public required BasicLink InterOrganizationalRelationType { get; init; }
    public BasicLink? GeographicEntity { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public decimal? MoneyInvolved { get; init; }
    public required int NumberOfChildrenInvolved { get; init; }
    public string? Description { get; init; }
    public required int Direction { get; init; }
}
