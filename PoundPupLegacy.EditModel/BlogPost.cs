namespace PoundPupLegacy.EditModel;


public interface BlogPost : SimpleTextNode
{

}
public abstract record BlogPostBase : SimpleTextNodeBase, BlogPost
{

}

[JsonSerializable(typeof(ExistingBlogPost))]
public partial class ExistingBlogPostJsonContext : JsonSerializerContext { }

public sealed record ExistingBlogPost : BlogPostBase, ExistingNode
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }

}

[JsonSerializable(typeof(NewBlogPost))]
public partial class NewBlogPostJsonContext : JsonSerializerContext { }

public sealed record NewBlogPost : BlogPostBase, NewNode
{
}
