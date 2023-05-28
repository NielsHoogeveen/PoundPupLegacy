namespace PoundPupLegacy.CreateModel;

public abstract record BlogPost : SimpleTextNode
{
    private BlogPost() { }
    public required SimpleTextNodeDetails SimpleTextNodeDetails { get; init; }
    public abstract NodeIdentification NodeIdentification { get; }
    public abstract NodeDetails NodeDetails { get; }
    public abstract T Match<T>(Func<BlogPostToCreate, T> create, Func<BlogPostToUpdate, T> update);
    public abstract void Match(Action<BlogPostToCreate> create, Action<BlogPostToUpdate> update);

    public sealed record BlogPostToCreate : BlogPost, NodeToCreate
    {
        public required NodeIdentification.NodeIdentificationForCreate NodeIdentificationForCreate { get; init; }
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }

        public override NodeIdentification NodeIdentification => NodeIdentificationForCreate;

        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public override T Match<T>(Func<BlogPostToCreate, T> create, Func<BlogPostToUpdate, T> update)
        {
            return create(this);
        }
        public override void Match(Action<BlogPostToCreate> create, Action<BlogPostToUpdate> update)
        {
            create(this);
        }
    }
    public sealed record BlogPostToUpdate : BlogPost, NodeToUpdate
    {
        public override NodeIdentification NodeIdentification => NodeIdentificationForUpdate;

        public override NodeDetails NodeDetails => NodeDetailsForUpdate;

        public required NodeIdentification.NodeIdentificationForUpdate NodeIdentificationForUpdate { get; init; }
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override T Match<T>(Func<BlogPostToCreate, T> create, Func<BlogPostToUpdate, T> update)
        {
            return update(this);
        }
        public override void Match(Action<BlogPostToCreate> create, Action<BlogPostToUpdate> update)
        {
            update(this);
        }
    }
}
