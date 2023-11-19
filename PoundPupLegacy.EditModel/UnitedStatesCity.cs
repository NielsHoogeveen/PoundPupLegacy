namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(UnitedStatesCity))]
[JsonSerializable(typeof(UnitedStatesCity.ToCreate))]
[JsonSerializable(typeof(UnitedStatesCity.ToUpdate))]
public partial class UnitedStatesCityJsonContext : JsonSerializerContext { }


public abstract record UnitedStatesCity : Nameable, ResolvedNode, Node<UnitedStatesCity.ToUpdate, UnitedStatesCity.ToCreate>, Resolver<UnitedStatesCity.ToUpdate, UnitedStatesCity.ToCreate, Unit>
{
    private UnitedStatesCity() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public required int Population { get; set; }
    public required double Density { get; set; }
    public required bool Military { get; set; }
    public required bool Incorporated { get; set; }
    public required decimal Latitude { get; set; }
    public required decimal Longitude { get; set; }
    public required string Timezone { get; set; }
    public required int CountyId { get; set; }
    public required string CountyName { get; set; }
    public required string SimpleName { get; set; }
    public abstract T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required NameableDetails NameableDetails { get; init; }
    public abstract NodeDetails NodeDetails { get; }
    public sealed record ToUpdate : UnitedStatesCity, ExistingNode
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
    public sealed record ToCreate : UnitedStatesCity, ResolvedNewNode
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



