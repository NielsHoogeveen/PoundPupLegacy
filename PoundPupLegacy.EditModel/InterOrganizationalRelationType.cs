namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(InterOrganizationalRelationType))]
[JsonSerializable(typeof(InterOrganizationalRelationType.ToCreate))]
[JsonSerializable(typeof(InterOrganizationalRelationType.ToUpdate))]
public partial class InterOrganizationalRelationTypeJsonContext : JsonSerializerContext { }


public abstract record InterOrganizationalRelationType : Nameable, ResolvedNode, Node<InterOrganizationalRelationType.ToUpdate, InterOrganizationalRelationType.ToCreate>, Resolver<InterOrganizationalRelationType.ToUpdate, InterOrganizationalRelationType.ToCreate, Unit>
{
    private InterOrganizationalRelationType() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;

    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required bool IsSymmetric { get; set; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : InterOrganizationalRelationType, ExistingNode
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
    public sealed record ToCreate : InterOrganizationalRelationType, ResolvedNewNode
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



