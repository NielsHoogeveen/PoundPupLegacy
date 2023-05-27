namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingAbuseCase))]
public partial class ExistingAbuseCaseJsonContext : JsonSerializerContext { }

[JsonSerializable(typeof(NewAbuseCase))]
public partial class NewAbuseCaseJsonContext : JsonSerializerContext { }

public interface AbuseCase : Case, ResolvedNode
{
    int ChildPlacementTypeId { get; set; }
    int? FamilySizeId { get; set; }
    bool? HomeschoolingInvolved { get; set; }
    bool? FundamentalFaithInvolved { get; set; }
    bool? DisabilitiesInvolved { get; set; }
}

public sealed record NewAbuseCase : NewCaseBase, ResolvedNewNode, AbuseCase
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
public sealed record ExistingAbuseCase : ExistingCaseBase, AbuseCase
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
