namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(InterPersonalRelationType))]
[JsonSerializable(typeof(InterPersonalRelationType.ToCreate))]
[JsonSerializable(typeof(InterPersonalRelationType.ToUpdate))]
public partial class InterPersonalRelationTypeJsonContext : JsonSerializerContext { }


public abstract record InterPersonalRelationType : Nameable, ResolvedNode, Node<InterPersonalRelationType.ToUpdate, InterPersonalRelationType.ToCreate>, Resolver<InterPersonalRelationType.ToUpdate, InterPersonalRelationType.ToCreate, Unit>
{
    private InterPersonalRelationType() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public required bool IsSymmetric { get; set; }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : InterPersonalRelationType, ExistingNode
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
    public sealed record ToCreate : InterPersonalRelationType, ResolvedNewNode
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



