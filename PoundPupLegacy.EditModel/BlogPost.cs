namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(BlogPost.ToUpdate), TypeInfoPropertyName = "BlogPostToUpdate")]
[JsonSerializable(typeof(Tenant.ToUpdate), TypeInfoPropertyName = "TenantToUpdate")]
[JsonSerializable(typeof(List<Tenant.ToUpdate>), TypeInfoPropertyName = "TenantListToUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
[JsonSerializable(typeof(Tags.ToUpdate), TypeInfoPropertyName = "TagsToUpdate")]
[JsonSerializable(typeof(List<Tags.ToUpdate>), TypeInfoPropertyName = "TagsListToUpdate")]
public partial class BlogPostToUpdateJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(BlogPost.ToCreate), TypeInfoPropertyName = "BlogPostToCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForCreate")]
[JsonSerializable(typeof(Tenant.ToCreate), TypeInfoPropertyName = "TenantToCreate")]
[JsonSerializable(typeof(List<Tenant.ToCreate>), TypeInfoPropertyName = "TenantListToCreate")]
[JsonSerializable(typeof(Tags.ToCreate), TypeInfoPropertyName = "TagsToCreate")]
[JsonSerializable(typeof(List<Tags.ToCreate>), TypeInfoPropertyName = "TagsListToCreate")]
public partial class BlogPostToCreateJsonContext : JsonSerializerContext { }

public abstract record BlogPost : SimpleTextNode, ResolvedNode, Node<BlogPost.ToUpdate, BlogPost.ToCreate>, Resolver<BlogPost.ToUpdate, BlogPost.ToCreate, Unit>
{
    private BlogPost() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
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

