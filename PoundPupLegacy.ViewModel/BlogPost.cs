namespace PoundPupLegacy.ViewModel;

public record struct BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime ChangedDateTime { get; set; }
    public string Text { get; set; }
    public Author Author { get; set; }
    public Tag[] Tags { get; set; }
    public SeeAlsoBoxElement[] SeeAlsoBoxElements { get; set; }
    public Comment[] Comments { get; set; }

}
