namespace PoundPupLegacy.ViewModel;

public record Discussion : SimpleTextNode
{
    public int Id { get; set; }
    public int NodeTypeId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Authoring Authoring { get; set; }
    public bool HasBeenPublished { get; set; }
    public Link[] Tags { get; set; }
    public Link[] SeeAlsoBoxElements { get; set; }
    public Comment[] Comments { get; set; }

    public Link[] BreadCrumElements { get; set; }

}
