namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(PersonOrganizationRelationType))]
[JsonSerializable(typeof(PersonOrganizationRelationType.ToCreate))]
[JsonSerializable(typeof(PersonOrganizationRelationType.ToUpdate))]
public partial class PersonOrganizationRelationTypeJsonContext : JsonSerializerContext { }


public abstract record PersonOrganizationRelationType : Nameable, ResolvedNode, Node<PersonOrganizationRelationType.ToUpdate, PersonOrganizationRelationType.ToCreate>, Resolver<PersonOrganizationRelationType.ToUpdate, PersonOrganizationRelationType.ToCreate, Unit>
{
    private PersonOrganizationRelationType() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;

    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : PersonOrganizationRelationType, ExistingNode
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
    public sealed record ToCreate : PersonOrganizationRelationType, ResolvedNewNode
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



