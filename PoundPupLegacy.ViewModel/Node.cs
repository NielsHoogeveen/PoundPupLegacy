namespace PoundPupLegacy.ViewModel;

public interface Node
{
    public int Id { get; set; }
    public int NodeTypeId { get; set; }
    public string Title { get; set; }
    public Authoring Authoring { get; set; }
    public bool HasBeenPublished { get; set; }
    public Link[] Tags { get; set; }
    public Comment[] Comments { get; set; }
    public Link[] BreadCrumElements { get; set; }

}
