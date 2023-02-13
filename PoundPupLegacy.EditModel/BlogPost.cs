namespace PoundPupLegacy.EditModel;

public record BlogPost: SimpleTextNode
{
    public required int NodeId { get; init; }

    public required int UrlId { get; init; }

    public required string Title { get; set; }

    public required string Text { get; set; }

    private List<Tag> tags = new List<Tag>();

    public List<Tag> Tags { 
        get => tags;
        init 
        { 
            if(value is not null)
            {
                tags = value;
            } 
        }
    }
}
