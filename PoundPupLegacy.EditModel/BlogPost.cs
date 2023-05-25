namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingBlogPost))]
public partial class ExistingBlogPostJsonContext : JsonSerializerContext { }

public interface BlogPost : SimpleTextNode, ResolvedNode
{

}
public abstract record BlogPostBase : SimpleTextNodeBase, BlogPost
{

}

public sealed record ExistingBlogPost : BlogPostBase, ExistingNode
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }

}

public sealed record NewBlogPost : BlogPostBase, ResolvedNewNode
{
}
