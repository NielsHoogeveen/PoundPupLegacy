namespace PoundPupLegacy.EditModel;


public interface BlogPost: SimpleTextNode
{

}
public record BlogPostBase: SimpleTextNodeBase, BlogPost
{

}

[JsonSerializable(typeof(ExistingBlogPost))]
public partial class ExistingBlogPostJsonContext : JsonSerializerContext { }

public record ExistingBlogPost : BlogPostBase, ExistingNode
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }

}

[JsonSerializable(typeof(NewBlogPost))]
public partial class NewBlogPostJsonContext : JsonSerializerContext { }

public record NewBlogPost : BlogPostBase, NewNode
{
}
