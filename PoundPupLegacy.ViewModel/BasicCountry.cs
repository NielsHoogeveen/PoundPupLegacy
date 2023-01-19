namespace PoundPupLegacy.ViewModel;

public record struct BasicCountry : TopLevelCountry
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
}
