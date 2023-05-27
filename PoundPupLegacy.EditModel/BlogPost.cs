namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingBlogPost))]
public partial class ExistingBlogPostJsonContext : JsonSerializerContext { }

public interface BlogPost : SimpleTextNode, ResolvedNode
{

}

public sealed record ExistingBlogPost : ExistingSimpleTextNodeBase, BlogPost
{

}

public sealed record NewBlogPost : NewSimpleTextNodeBase, ResolvedNewNode, BlogPost
{
}
