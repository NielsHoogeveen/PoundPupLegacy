namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(AbuseCase.ToUpdate), TypeInfoPropertyName = "AbuseCaseToUpdate")]
[JsonSerializable(typeof(LocatableDetails.ForUpdate), TypeInfoPropertyName = "LocatableDetailsForUpdate")]
[JsonSerializable(typeof(Location.ToUpdate), TypeInfoPropertyName = "LocationDetailsForUpdate")]
[JsonSerializable(typeof(List<Location.ToUpdate>), TypeInfoPropertyName = "LocationDetailsListForUpdate")]
[JsonSerializable(typeof(NodeDetails.ForUpdate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class ExistingAbuseCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(AbuseCase.ToCreate))]
[JsonSerializable(typeof(LocatableDetails.ForCreate), TypeInfoPropertyName = "LocatableDetailsCreate")]
[JsonSerializable(typeof(Location.ToCreate), TypeInfoPropertyName = "LocationDetailsForCreate")]
[JsonSerializable(typeof(List<Location.ToCreate>), TypeInfoPropertyName = "LocationDetailsListForCreate")]
[JsonSerializable(typeof(NodeDetails.ForCreate), TypeInfoPropertyName = "NodeDetailsForUpdate")]
public partial class NewAbuseCaseJsonContext : JsonSerializerContext { }

public abstract record AbuseCase: Case, Locatable, Nameable, Node, Node<AbuseCase.ToUpdate, AbuseCase.ToCreate>, Resolver<AbuseCase.ToUpdate, AbuseCase.ToCreate, Unit>
{
    private AbuseCase() { }
    public Node<ToUpdate, ToCreate> Resolve(Unit data) => this;
    public abstract T Match<T>(Func<ToUpdate,T> existingItem, Func<ToCreate, T> newItem);
    public abstract void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem);
    public required AbuseCaseDetails AbuseCaseDetails { get; init; }
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }
    
    public sealed record ToCreate : AbuseCase, ResolvedNewNode, NewLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.ForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForCreate;
        public required LocatableDetails.ForCreate LocatableDetailsForCreate { get; init; }
        public override T Match<T>(Func<ToUpdate, T> existingItem, Func<ToCreate, T> newItem) {
            return newItem(this);
        }
        public override void Match(Action<ToUpdate> existingItem, Action<ToCreate> newItem)
        {
            newItem(this);
        }
    }
    public sealed record ToUpdate : AbuseCase, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.ForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => LocatableDetailsForUpdate;
        public required LocatableDetails.ForUpdate LocatableDetailsForUpdate { get; init; }
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
}

public sealed record AbuseCaseDetails
{
    public required int ChildPlacementTypeId { get; set; }
    public int? FamilySizeId { get; set; }
    public bool? HomeschoolingInvolved { get; set; }
    public bool? FundamentalFaithInvolved { get; set; }
    public bool? DisabilitiesInvolved { get; set; }

    private List<TypeOfAbuse> typesOfAbuse = new List<TypeOfAbuse>();
    public List<TypeOfAbuse> TypesOfAbuse {
        get => typesOfAbuse;
        set => typesOfAbuse = value ?? new List<TypeOfAbuse>();
    }
    private List<TypeOfAbuser> typesOfAbuser = new List<TypeOfAbuser>();
    public List<TypeOfAbuser> TypesOfAbuser {
        get => typesOfAbuser;
        set => typesOfAbuser = value ?? new List<TypeOfAbuser>();
    }
    public required ChildPlacementType[] ChildPlacementTypesToSelectFrom { get; init; }
    public required FamilySize[] FamilySizesToSelectFrom { get; init; }
    public required TypeOfAbuse[] TypesOfAbuseToSelectFrom { get; init; }
    public required TypeOfAbuser[] TypesOfAbuserToSelectFrom { get; init; }
}
