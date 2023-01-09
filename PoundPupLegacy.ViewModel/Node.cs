namespace PoundPupLegacy.ViewModel;

public interface Node
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Authoring Authoring { get; set; }
    public Tag[] Tags { get; set; }
    public Comment[] Comments { get; set; }
    public BreadCrumElement[] BreadCrumElements { get; set; }

}
