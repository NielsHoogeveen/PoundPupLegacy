namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(BlogPost.ExistingBlogPost))]
public partial class ExistingBlogPostJsonContext : JsonSerializerContext { }

public abstract record BlogPost : SimpleTextNode, ResolvedNode
{
    private BlogPost() { }
    public abstract T Match<T>(Func<ExistingBlogPost, T> existingItem, Func<NewBlogPost, T> newItem);
    public abstract void Match(Action<ExistingBlogPost> existingItem, Action<NewBlogPost> newItem);
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeDetails NodeDetails { get;  }
    public sealed record ExistingBlogPost : BlogPost, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingBlogPost, T> existingItem, Func<NewBlogPost, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingBlogPost> existingItem, Action<NewBlogPost> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record NewBlogPost: BlogPost, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override T Match<T>(Func<ExistingBlogPost, T> existingItem, Func<NewBlogPost, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ExistingBlogPost> existingItem, Action<NewBlogPost> newItem)
        {
            newItem(this);
        }
    }
}

