namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PartyPoliticalEntityRelationType))]
[JsonSerializable(typeof(PartyPoliticalEntityRelationType.ToCreate))]
[JsonSerializable(typeof(PartyPoliticalEntityRelationType.ToUpdate))]
public partial class PartyPoliticalEntityRelationTypeJsonContext : JsonSerializerContext { }


public abstract record PartyPoliticalEntityRelationType : Nameable, ResolvedNode, Node<PartyPoliticalEntityRelationType.ToUpdate, PartyPoliticalEntityRelationType.ToCreate>, Resolver<PartyPoliticalEntityRelationType.ToUpdate, PartyPoliticalEntityRelationType.ToCreate, Unit>
{
    private PartyPoliticalEntityRelationType() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;

    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : PartyPoliticalEntityRelationType, ExistingNode
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
    public sealed record ToCreate : PartyPoliticalEntityRelationType, ResolvedNewNode
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



