namespace PoundPupLegacy.EditModel;

public interface Node
{
    public int NodeId { get; init; }

    public int UrlId { get; init; }

    public string Title { get; set; }
    public List<Tag> Tags { get; init;  }
}
