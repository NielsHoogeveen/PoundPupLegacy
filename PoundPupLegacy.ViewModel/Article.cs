namespace PoundPupLegacy.ViewModel;

public record Article: SimpleTextNode
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Authoring Authoring { get; set; }

    public bool HasBeenPublished { get; set; }
    public Tag[] Tags { get; set; }
    public SeeAlsoBoxElement[] SeeAlsoBoxElements { get; set; }
    public Comment[] Comments { get; set; }

    public BreadCrumElement[] BreadCrumElements { get; set; }

}
