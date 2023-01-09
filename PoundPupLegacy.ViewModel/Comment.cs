namespace PoundPupLegacy.ViewModel;

public record struct Comment
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int NodeStatusId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public Authoring Authoring { get; set; }  
    public Comment[] Comments { get; set;}
    
}
