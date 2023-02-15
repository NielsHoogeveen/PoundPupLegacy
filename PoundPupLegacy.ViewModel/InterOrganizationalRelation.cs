namespace PoundPupLegacy.ViewModel;

public record InterOrganizationalRelation
{
    public Link OrganizationFrom { get; set; }
    public Link OrganizationTo { get; set; }
    public Link InterOrganizationalRelationType { get; set; }
    public Link? GeographicEntity { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public decimal? MoneyInvolved { get; set; }
    public int? NumberOfChildrenInvolved { get; set; }
    public string? Description { get; set; }
    public int Direction { get; set; }
}
