namespace PoundPupLegacy.ViewModel;

public record BasicCountry : TopLevelCountry
{
    public string Description { get; set; }
    public int Id { get; set; }
    public string Title { get; set; }
    public Authoring Authoring { get; set; }
    public bool HasBeenPublished { get; set; }
    public Tag[] Tags { get; set; }
    public Comment[] Comments { get; set; }
    public BreadCrumElement[] BreadCrumElements { get; set; }
    public AdoptionImports AdoptionImports { get; set; }
    public DocumentListItem[] Documents { get; set; }
    public OrganizationTypeWithOrganizations[] OrganizationTypes { get; set; }

    public CountrySubdivisionType[] SubdivisionTypes { get; set; }
}
