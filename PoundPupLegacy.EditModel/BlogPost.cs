namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(BlogPost.ToUpdate), TypeInfoPropertyName = "BlogPostToUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingBlogPostJsonContext : JsonSerializerContext { }

public abstract record BlogPost : SimpleTextNode, ResolvedNode
{
    private BlogPost() { }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeDetails NodeDetails { get;  }
    public sealed record ToUpdate : BlogPost, ExistingNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            existingItem(this);
        }
    }
    public sealed record ToCreate: BlogPost, ResolvedNewNode
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }

        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem)
        {
            return newItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            newItem(this);
        }
    }
}

