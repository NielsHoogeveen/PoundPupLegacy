using System.Reflection.Metadata.Ecma335;

namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(AbuseCase.ExistingAbuseCase))]
public partial class ExistingAbuseCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(AbuseCase.NewAbuseCase))]
public partial class NewAbuseCaseJsonContext : JsonSerializerContext { }

public abstract record AbuseCase: Case, Locatable, Nameable, Node
{
    private AbuseCase() { }
    public abstract T Match<T>(Func<ExistingAbuseCase,T> existingItem, Func<NewAbuseCase, T> newItem);
    public abstract void Match(Action<ExistingAbuseCase> existingItem, Action<NewAbuseCase> newItem);
    public required AbuseCaseDetails AbuseCaseDetails { get; init; }
    public required CaseDetails CaseDetails { get; init; }
    public required NameableDetails NameableDetails { get; init; }
    public abstract LocatableDetails LocatableDetails { get; }
    public abstract NodeDetails NodeDetails { get; }

    public sealed record NewAbuseCase : AbuseCase, ResolvedNewNode, NewLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForCreate;
        public required NodeDetails.NodeDetailsForCreate NodeDetailsForCreate { get; init; }
        public override LocatableDetails LocatableDetails => NewLocatableDetails;
        public required LocatableDetails.NewLocatableDetails NewLocatableDetails { get; init; }
        public override T Match<T>(Func<ExistingAbuseCase, T> existingItem, Func<NewAbuseCase, T> newItem) {
            return newItem(this);
        }
        public override void Match(Action<ExistingAbuseCase> existingItem, Action<NewAbuseCase> newItem)
        {
            newItem(this);
        }
    }
    public sealed record ExistingAbuseCase : AbuseCase, ExistingLocatable
    {
        public override NodeDetails NodeDetails => NodeDetailsForUpdate;
        public required NodeDetails.NodeDetailsForUpdate NodeDetailsForUpdate { get; init; }
        public override LocatableDetails LocatableDetails => ExistingLocatableDetails;
        public required LocatableDetails.ExistingLocatableDetails ExistingLocatableDetails { get; init; }
        public required NodeIdentification NodeIdentification { get; init; }
        public override T Match<T>(Func<ExistingAbuseCase, T> existingItem, Func<NewAbuseCase, T> newItem)
        {
            return existingItem(this);
        }
        public override void Match(Action<ExistingAbuseCase> existingItem, Action<NewAbuseCase> newItem)
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
