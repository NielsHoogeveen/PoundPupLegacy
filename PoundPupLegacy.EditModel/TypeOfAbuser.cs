namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(TypeOfAbuser))]
[JsonSerializable(typeof(TypeOfAbuser.ToCreate))]
[JsonSerializable(typeof(TypeOfAbuser.ToUpdate))]
public partial class TypeOfAbuserJsonContext : JsonSerializerContext { }


public abstract record TypeOfAbuser : Nameable, ResolvedNode, Node<TypeOfAbuser.ToUpdate, TypeOfAbuser.ToCreate>, Resolver<TypeOfAbuser.ToUpdate, TypeOfAbuser.ToCreate, Unit>
{
    private TypeOfAbuser() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;

    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : TypeOfAbuser, ExistingNode
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
    public sealed record ToCreate : TypeOfAbuser, ResolvedNewNode
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



